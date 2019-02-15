// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpecFlow.GeneratedTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ScrutinMajoritaireFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ScrutinMajoritaire", "    In order to avoid silly mistakes\r\n    As a math idiot\r\n    I want to be told " +
                    "the sum of two numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "ScrutinMajoritaire")))
            {
                SpecFlow.GeneratedTests.Features.ScrutinMajoritaireFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
            testRunner.Given("Un nouveau scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ScrutinMajoritaire")]
        public virtual void ScrutinMajoritaireUnElecteurEtUnVainqueur()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Scrutin majoritaire un électeur et un vainqueur", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nom"});
            table25.AddRow(new string[] {
                        "candidat 1"});
            table25.AddRow(new string[] {
                        "candidat 2"});
            testRunner.Given("les options suivantes pour le scrutin \"scrutin 1\"", ((string)(null)), table25, "Given ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("je clôture le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("\"candidat 1\" est désigné comme vainqueur", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("le résultat est valide", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Option",
                        "Nombre de vote",
                        "pourcentage"});
            table26.AddRow(new string[] {
                        "candidat 1",
                        "1",
                        "100"});
            table26.AddRow(new string[] {
                        "candidat 2",
                        "0",
                        "0"});
            testRunner.And("j\'obtiens le résultat suivant", ((string)(null)), table26, "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ScrutinMajoritaire")]
        public virtual void ScrutinMajoritaireSansVainqueurAvecSecondTour()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Scrutin majoritaire sans vainqueur avec second tour", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nom"});
            table27.AddRow(new string[] {
                        "candidat 1"});
            table27.AddRow(new string[] {
                        "candidat 2"});
            table27.AddRow(new string[] {
                        "candidat 3"});
            testRunner.Given("les options suivantes pour le scrutin \"scrutin 1\"", ((string)(null)), table27, "Given ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 2\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 2\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 3\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("je clôture le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("il n\'y a pas de vainqueur", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("le résultat est valide", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                        "Option",
                        "Nombre de vote",
                        "pourcentage"});
            table28.AddRow(new string[] {
                        "candidat 1",
                        "2",
                        "40"});
            table28.AddRow(new string[] {
                        "candidat 2",
                        "2",
                        "40"});
            table28.AddRow(new string[] {
                        "candidat 3",
                        "1",
                        "20"});
            testRunner.And("j\'obtiens le résultat suivant", ((string)(null)), table28, "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ScrutinMajoritaire")]
        public virtual void ScrutinMajoritaireSansVainqueurEtSansSecondTour()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Scrutin majoritaire sans vainqueur et sans second tour", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nom"});
            table29.AddRow(new string[] {
                        "candidat 1"});
            table29.AddRow(new string[] {
                        "candidat 2"});
            table29.AddRow(new string[] {
                        "candidat 3"});
            testRunner.Given("les options suivantes pour le scrutin \"scrutin 1\"", ((string)(null)), table29, "Given ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 2\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 3\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("je clôture le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("il n\'y a pas de vainqueur", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("le résultat n\'est pas valide", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                        "Option",
                        "Nombre de vote",
                        "pourcentage"});
            table30.AddRow(new string[] {
                        "candidat 1",
                        "2",
                        "50"});
            table30.AddRow(new string[] {
                        "candidat 2",
                        "1",
                        "25"});
            table30.AddRow(new string[] {
                        "candidat 3",
                        "1",
                        "25"});
            testRunner.And("j\'obtiens le résultat suivant", ((string)(null)), table30, "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ScrutinMajoritaire")]
        public virtual void ScrutinMajoritaireOuLeVoteBlancGagne()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Scrutin majoritaire ou le vote blanc gagne", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            TechTalk.SpecFlow.Table table31 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nom"});
            table31.AddRow(new string[] {
                        "candidat 1"});
            testRunner.Given("les options suivantes pour le scrutin \"scrutin 1\"", ((string)(null)), table31, "Given ");
            testRunner.And("un electeur vote blanc pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("je clôture le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("il n\'y a pas de vainqueur", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("le résultat n\'est pas valide", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            TechTalk.SpecFlow.Table table32 = new TechTalk.SpecFlow.Table(new string[] {
                        "Option",
                        "Nombre de vote",
                        "pourcentage"});
            table32.AddRow(new string[] {
                        "Vote blanc",
                        "1",
                        "100"});
            table32.AddRow(new string[] {
                        "candidat 1",
                        "0",
                        "0"});
            testRunner.And("j\'obtiens le résultat suivant", ((string)(null)), table32, "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ScrutinMajoritaire")]
        public virtual void ScrutinMajoritaireSecondTourMoinsDe50SansEgalite()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Scrutin majoritaire second tour moins de 50% sans egalite", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            TechTalk.SpecFlow.Table table33 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nom"});
            table33.AddRow(new string[] {
                        "candidat 1"});
            table33.AddRow(new string[] {
                        "candidat 2"});
            table33.AddRow(new string[] {
                        "candidat 3"});
            testRunner.Given("les options suivantes pour le scrutin \"scrutin 1\"", ((string)(null)), table33, "Given ");
            testRunner.And("on est au second tour du scrutin \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 2\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 3\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("je clôture le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("\"candidat 1\" est désigné comme vainqueur", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("le résultat est valide", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ScrutinMajoritaire")]
        public virtual void ScrutinMajoritaireSecondTourAvecEgalite()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Scrutin majoritaire second tour avec egalite", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            TechTalk.SpecFlow.Table table34 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nom"});
            table34.AddRow(new string[] {
                        "candidat 1"});
            table34.AddRow(new string[] {
                        "candidat 2"});
            testRunner.Given("les options suivantes pour le scrutin \"scrutin 1\"", ((string)(null)), table34, "Given ");
            testRunner.And("on est au second tour du scrutin \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 1\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 2\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("un electeur vote \"candidat 2\" pour le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("je clôture le scrutin majoritaire \"scrutin 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("il n\'y a pas de vainqueur", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("le résultat n\'est pas valide", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion