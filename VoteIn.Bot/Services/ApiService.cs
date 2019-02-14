using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoteIn.Bot.Dialogs;
using VoteIn.Model.Models;
using VoteIn.Utils;

namespace VoteIn.Bot.Services
{
    public class ApiService
    {
        public async Task<Scrutin> CreatePoll(string nom, TypesScrutin? typeScrutin, string candidats, string user)
        {
            try
            {
                var client = new HttpClient();
                //get Id modeScrutin
                var response = await client.GetAsync("https://voteinback.azurewebsites.net/api/modeScrutin");
                var jsonstring = await response.Content.ReadAsStringAsync();

                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                var modeScrutins = JsonConvert.DeserializeObject<List<ModeScrutin>>(jsonstring);
                var selectedCode = typeScrutin == TypesScrutin.JugementMajoritaire ? Constantes.JUG_MAJ : (typeScrutin == TypesScrutin.VoteAlternatif ? Constantes.VOTE_ALTER : Constantes.SCRUTIN_MAJ);
                Scrutin scrutin = new Scrutin
                {
                    Auteur = user,
                    DateOuverture = DateTime.Now,
                    IdModeScrutin = modeScrutins.First(m => m.Code == selectedCode).Id,
                    Nom = nom,
                    Public = true,
                    Description = "Généré par le chatbot",
                    OptionScrutin = new List<OptionScrutin>()
                };

                foreach (var c in candidats.Split(','))
                {
                    scrutin.OptionScrutin.Add(new OptionScrutin
                    {
                        Option = new Option
                        {
                            Nom = c
                        }
                    });
                }

                var jsonString = JsonConvert.SerializeObject(scrutin);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                response = await client.PostAsync("https://voteinback.azurewebsites.net/api/scrutin", content);
                jsonstring = await response.Content.ReadAsStringAsync();
                scrutin = JsonConvert.DeserializeObject<Scrutin>(jsonstring);
                return scrutin;
            }
            catch (Exception ex)
            {

            }
            

            return null;
        }
    }
}