using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VoteIn.BL.Interfaces;
using VoteIn.DAL;
using VoteIn.Model.Models;
using VoteIn.Model.ViewModels;
using VoteIn.Utils;

namespace VoteIn.BL.Repositories
{

    public class EnveloppeRepository : Repository<Envelope>, IEnveloppeRepository
    {
        /// <summary>
        /// The suffrage repository
        /// </summary>
        private readonly ISuffrageRepository suffrageRepository;

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="EnveloppeRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="suffrageRepo">The suffrage repo.</param>
        public EnveloppeRepository(VoteInContext context, ISuffrageRepository suffrageRepo) : base(context)
        {
            suffrageRepository = suffrageRepo;
        }
        #endregion

        public class DejaVoteException : Exception
        {
        }

        #region Public Methods
        /// <summary>
        /// Determines whether the specified identifier electeur has voted.
        /// </summary>
        /// <param name="idElecteur">The identifier electeur.</param>
        /// <param name="token">The token.</param>
        /// <param name="envelope">The enveloppe.</param>
        /// <exception cref="VoteIn.BL.Repositories.EnveloppeRepository.DejaVoteException"></exception>
        public void HasVoted(int idElecteur, string token, Envelope envelope)
        {
            var el = Context.Voter.FirstOrDefault(e => e.Id == idElecteur);
            if (el == null && !string.IsNullOrEmpty(token))
            {
                el = Context.Voter.FirstOrDefault(e => e.Token == token);
            }

            var scrutin = Context.VotingProcess.Include("VotingProcessOption").First(sc => sc.Id == envelope.IdVotingProcess);

            if (scrutin.Public || !scrutin.Public && el != null && !el.HasVoted)
            {
                if (el != null)
                    el.HasVoted = true;

                Add(envelope);
                Save();
            }
            else
            {
                throw new DejaVoteException();
            }
        }
        /// <summary>
        /// Counts the vote.
        /// </summary>
        /// <param name="votingProcess">The scrutin.</param>
        public void CountVote(VotingProcess votingProcess)
        {
            var votes = new List<JObject>();
            var enveloppes = GetAll(filter: e => e.IdVotingProcess == votingProcess.Id);
            //Décoder la clé AES avec la clé privée RSA
            using (TextReader sr = new StringReader(votingProcess.GetPEMString()))
            {
                var pr = new PemReader(sr);
                var KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
                var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaParams);
                    foreach (var enveloppe in enveloppes)
                    {
                        try
                        {
                            var aesKey = Convert.FromBase64String(Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(enveloppe.Key), false)));

                            var counter = new byte[16];
                            for (var i = 0; i < 15; i++)
                            {
                                counter[i] = 0;
                            }
                            counter[15] = 1;

                            //Décoder le contenu avec la clé AES
                            var jsonBytes = CTREncryptBytes(Convert.FromBase64String(enveloppe.Content), aesKey, counter);
                            var jsonString = Encoding.UTF8.GetString(jsonBytes);

                            votes.Add(JObject.Parse(jsonString));
                            //TODO Delete(enveloppe);
                        }
                        catch
                        {
                            //TODO : Traiter le cas où l'on rencontre une enveloppe invalide.
                        }
                    }
                }
            }
            Save();
            foreach (var vote in votes)
            {
                switch (votingProcess.VotingProcessMode.Code)
                {
                    case Constantes.VOTE_ALTER:
                        suffrageRepository.AddVoteAlternatif(vote.ToObject<VoteAlternative>());
                        break;
                    case Constantes.VOTING_PROCESS_MAJ:
                        suffrageRepository.AddVoteScrutinMajoritaire(vote.ToObject<VoteMajoritaryVotingProcess>());
                        break;
                    case Constantes.JUG_MAJ:
                        suffrageRepository.AddVoteJugementMajoritaire(vote.ToObject<VoteMajoritaryJudgment>());
                        break;
                    case Constantes.CONDOR_RANDOM:
                        suffrageRepository.AddVoteAlternatif(vote.ToObject<VoteAlternative>());
                        break;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Aeses the encrypt.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="plainBytes">The plain bytes.</param>
        /// <returns></returns>
        private static byte[] AESEncrypt(byte[] key, byte[] plainBytes)
        {
            using (var myAes = new AesCryptoServiceProvider())
            {
                myAes.Key = key;
                myAes.Mode = CipherMode.ECB; // no iv
                myAes.Padding = PaddingMode.None;

                // Create a cryptor to perform the stream transform.
                var decryptor = myAes.CreateEncryptor(myAes.Key, null);

                var outputBuffer = new Byte[16];

                // Create the streams used for decryption. 
                decryptor.TransformBlock(plainBytes, 0, 16, outputBuffer, 0);


                return outputBuffer;

            }

        }
        /// <summary>
        /// CTRs the encrypt bytes.
        /// </summary>
        /// <param name="plainBytes">The plain bytes.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        private static byte[] CTREncryptBytes(byte[] plainBytes, byte[] key, byte[] iv)
        {
            // divid it into blocks then start to do CTR
            var cipheredBytes = new byte[plainBytes.Length];

            for (var i = 0; i < plainBytes.Length; i += 16)
            {


                if (i + 16 <= plainBytes.Length)
                {
                    // get the encryptuion of iv
                    var plainBlockBytes = new byte[16];

                    Buffer.BlockCopy(plainBytes, i, plainBlockBytes, 0, 16);

                    var encryptedBytes = AESEncrypt(key, iv);

                    // increase iv
                    IncrementAtIndex(iv, 15);

                    var xoredBytes = XOR(plainBlockBytes, encryptedBytes);

                    Buffer.BlockCopy(xoredBytes, 0, cipheredBytes, i, 16);

                }
                else
                {
                    // get the encryption of iv
                    var cipherBlockBytes = new byte[plainBytes.Length - i];

                    Buffer.BlockCopy(plainBytes, i, cipherBlockBytes, 0, plainBytes.Length - i);

                    var decryptedBytes = AESEncrypt(key, iv);

                    var xoredBytes = XOR(cipherBlockBytes, decryptedBytes);

                    Buffer.BlockCopy(xoredBytes, 0, cipheredBytes, i, plainBytes.Length - i);
                }



            }

            return cipheredBytes;

        }
        /// <summary>
        /// Increments at index.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        private static void IncrementAtIndex(byte[] array, int index)
        {

            if (array[index] == byte.MaxValue)
            {
                array[index] = 0;
                if (index > 0)
                    IncrementAtIndex(array, index - 1);
            }
            else
            {
                array[index]++;
            }
        }
        /// <summary>
        /// Xors the specified buffer1.
        /// </summary>
        /// <param name="buffer1">The buffer1.</param>
        /// <param name="buffer2">The buffer2.</param>
        /// <returns></returns>
        private static byte[] XOR(IReadOnlyList<byte> buffer1, IReadOnlyList<byte> buffer2)
        {
            var length = buffer1.Count <= buffer2.Count
                ? buffer1.Count
                : buffer2.Count;

            var result = new byte[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = (byte)(buffer1[i] ^ buffer2[i]);
            }
            return result;

        }
        #endregion
    }

}
