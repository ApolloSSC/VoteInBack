using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VoteIn.Model.Models
{
    [Table("VOTING_PROCESS")]
    public class VotingProcess
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Column("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [Column("GUID")]
        public Guid Guid { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VotingProcess"/> is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if public; otherwise, <c>false</c>.
        /// </value>
        [Column("PUBLIC")]
        public bool Public { get; set; }
        /// <summary>
        /// Gets the nb votes.
        /// </summary>
        /// <value>
        /// The nb votes.
        /// </value>
        [NotMapped]
        public int NbVotes {
            get
            {
                if (Envelope?.Count > 0)
                {
                    return Envelope.Count;
                }
                if (Suffrage != null && Suffrage.Count > 0)
                {
                    return Suffrage.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Gets or sets the closing date.
        /// </summary>
        /// <value>
        /// The closing date.
        /// </value>
        [Column("CLOSING_DATE")]
        public DateTime? ClosingDate { get; set; }
        /// <summary>
        /// Gets or sets the opening date.
        /// </summary>
        /// <value>
        /// The opening date.
        /// </value>
        [Column("OPENING_DATE")]
        public DateTime OpeningDate { get; set; }
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [Column("AUTHOR")]
        public string Author { get; set; }
        /// <summary>
        /// Gets or sets the author mail.
        /// </summary>
        /// <value>
        /// The author mail.
        /// </value>
        [Column("AUTHOR_MAIL")]
        public string AuthorMail { get; set; }
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        [Column("PUBLIC_KEY")]
        public string PublicKey { get; set; }
        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        [JsonIgnore]
        [Column("PRIVATE_KEY")]
        public string PrivateKey { get; set; }
        /// <summary>
        /// Gets or sets my private key.
        /// </summary>
        /// <value>
        /// My private key.
        /// </value>
        [NotMapped]
        public string MyPrivateKey { get; set; }
        /// <summary>
        /// Gets or sets the identifier voting process mode.
        /// </summary>
        /// <value>
        /// The identifier voting process mode.
        /// </value>
        [Column("ID_VOTING_PROCESS_MODE")]
        public int IdVotingProcessMode { get; set; }
        /// <summary>
        /// Gets or sets the identifier previous voting process.
        /// </summary>
        /// <value>
        /// The identifier previous voting process.
        /// </value>
        [JsonIgnore]
        [Column("ID_PREVIOUS_VOTING_PROCESS")]
        public int? IdPreviousVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier previous voting process.
        /// </summary>
        /// <value>
        /// The unique identifier previous voting process.
        /// </value>
        [Column("GUID_PREVIOUS_VOTING_PROCESS")]
        public Guid GuidPreviousVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the identifier user.
        /// </summary>
        /// <value>
        /// The identifier user.
        /// </value>
        [Column("ID_USER")]
        public string IdUser { get; set; }
        /// <summary>
        /// Gets or sets the voting process mode.
        /// </summary>
        /// <value>
        /// The voting process mode.
        /// </value>
        [ForeignKey("IdVotingProcessMode")]
        public VotingProcessMode VotingProcessMode { get; set; }
        /// <summary>
        /// Gets or sets the previous voting process.
        /// </summary>
        /// <value>
        /// The previous voting process.
        /// </value>
        [ForeignKey("IdPreviousVotingProcess")]
        public VotingProcess PreviousVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [ForeignKey("IdUser")]
        public User User { get; set; }
        /// <summary>
        /// Gets or sets the voting process option.
        /// </summary>
        /// <value>
        /// The voting process option.
        /// </value>
        public virtual ICollection<VotingProcessOption> VotingProcessOption { get; set; }
        /// <summary>
        /// Gets or sets the suffrage.
        /// </summary>
        /// <value>
        /// The suffrage.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Suffrage> Suffrage { get; set; }
        /// <summary>
        /// Gets or sets the envelope.
        /// </summary>
        /// <value>
        /// The envelope.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Envelope> Envelope { get; set; }
        /// <summary>
        /// Gets or sets the voter.
        /// </summary>
        /// <value>
        /// The voter.
        /// </value>
        public virtual ICollection<Voter> Voter { get; set; }
        /// <summary>
        /// Copies for next turn.
        /// </summary>
        /// <returns></returns>
        public VotingProcess CopyForNextTurn() => new VotingProcess
        {
            Name = Name,
            Description = Description,
            Public = Public,
            Author = Author,
            OpeningDate = DateTime.Now,
            IdVotingProcessMode = IdVotingProcessMode,
            IdPreviousVotingProcess = Id,
            GuidPreviousVotingProcess = Guid,
            AuthorMail = AuthorMail,
            VotingProcessMode = VotingProcessMode,
            PreviousVotingProcess = this
        };
        /// <summary>
        /// Gets the pem string.
        /// </summary>
        /// <returns></returns>
        public string GetPEMString()
        {
            var chunks = StringChunks(PrivateKey, 64);
            var outStr = chunks.Aggregate("-----BEGIN RSA PRIVATE KEY-----\n", (current, chunk) => current + (chunk + "\n"));
            outStr += "-----END RSA PRIVATE KEY-----";
            return outStr;
        }
        /// <summary>
        /// Strings the chunks.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        private static IEnumerable<string> StringChunks(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize + 1)
                .Select(i => str.Substring(i * chunkSize, Math.Min(chunkSize, str.Length - i * chunkSize)));
        }
    }
}
