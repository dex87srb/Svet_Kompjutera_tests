using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using static System.Console;

namespace Svet_Kompjutera.Tests
{
    public class Tests //filteri prvi(sve opcije) i drugi(prva opcija)
    {      
        IWebDriver driver;
               
        string[] keyWordsArray = { "Najave vot", "tru vot", "Uputstvo tru","Uputstvo upu", "Uputstvo za upotrebu", "Uputstvo z" };

        //naslovi iz rezultata pretrage koji trebaju da se pojave i sluze radi poredjenja sa actual rezultatima
        string[] arrayCompareTitle = {                                 
                                     "Žao nam je - nema rezultata. Molimo, pokušajte s nekim drugim tekstom.",
                                     "Uputstvo za upotrebu foruma (FAQ)",
                                     "Najave",
                                     "Uputstvo",
                                     "Uputstvo za upotrebu"
                                     };
        //tekstovi sadrzaja iz rezultata pretrage koji trebaju da se pojave i sluze radi poredjenja sa actual rezultatima
        string[] arrayCompareText =
        {    
              "Šta su najave?",
              "Ovde æete naæi odgovore na pitanja o tome kako se rukuje ovim forumom. Da biste našli ono što vas zanima, koristite linkove iz sadržaja ili pretraživanje.",
              "Šta je forum?"
        };

        int[] optionFilterOne = { 1, 2, 3 };

       string[] xpathTitle = {                               
                                "//td[contains(@class,'tcat')]//u[contains(text(),'Najave')]", 
                                "//*[contains(text(),'Žao nam je - nema rezultata. Molimo, pokušajte s nekim drugim tekstom.')]", 
                                "/html/body/div/div/div/table[4]/tbody/tr[1]/td",
                                "//td[contains(@class,'tcat')]//u[contains(text(),'Uputstvo')]"                          
                              };
        string[] xpathText =
                              {
                                "//td[contains(@class,'alt1')]//p//b[contains(text(),'Šta su ')]",
                                "//td[contains(text(),'Ovde æete naæi odgovore na pitanja o tome kako se rukuje ovim forumom. Da biste našli ono što vas zanima, koristite linkove iz sadržaja ili pretraživanje.')]",
                                "//td[contains(@class,'alt1')]//p//b[contains(text(),'Šta je forum?')]"
                              };

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        }
     
        public void Help_Srch(string[] arrayCompText, string[] arrayCompTitle, string keyCom, string xpathTitle, string xpathText, int optionFiltOne)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.sk.rs/forum/index.php");

            IWebElement element = driver.FindElement(By.XPath("//a[contains(@href,'faq.php')]"));
            element.Click();

            //Search
            driver.FindElement(By.XPath("//*[@id='collapseobj_searchfaq']/tr/td/div[1]/div/table/tbody/tr/td[1]/input")).SendKeys(keyCom);
            driver.FindElement(By.XPath(string.Format("//*[@id='collapseobj_searchfaq']/tr/td/div[1]/div/table/tbody/tr/td[2]/select/option[{0}]",optionFiltOne))).Click();
            driver.FindElement(By.XPath("//*[@id='collapseobj_searchfaq']/tr/td/div[1]/div/table/tbody/tr/td[3]/select/option[1]")).Click();

            driver.FindElement(By.XPath("//*[@id='collapseobj_searchfaq']/tr/td/div[2]/input[1]")).Click();

            IWebElement elemenTitle = driver.FindElement(By.XPath(xpathTitle));
            IWebElement elemenText = driver.FindElement(By.XPath(xpathText));

            //po filteru: po naslovima i tekstu, uporedjivanje search keyword i teksta iz rezultata          
            /* string[] arrayText = elemenText.Text.Split(',', ' ', '.','!','?');

            foreach (var item in arrayText)
            {
                string[] n = keyCom.Split(' ');
                foreach (var item2 in n)
                {
                    if (item.ToUpper() == item2.ToUpper())
                    {
                        Write("nesto");
                    }
                }
               
            }*/
                                                
            foreach (var item in arrayCompTitle)
            {
                if (elemenTitle.Text == item)
                {
                    foreach (var item2 in arrayCompText)
                    {
                        if (elemenText.Text==item2)
                        {
                            Assert.Pass();
                            break;
                        }                     
                    }
                }                        
            }

            Assert.Fail();         
        }

        public void Help_Srch(string[] arrayCompTitle, string keyCom, string xpathTitle, int optionFiltOne) 
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.sk.rs/forum/index.php");

            IWebElement element = driver.FindElement(By.XPath("//a[contains(@href,'faq.php')]"));
            element.Click();

            //Search
            driver.FindElement(By.XPath("//*[@id='collapseobj_searchfaq']/tr/td/div[1]/div/table/tbody/tr/td[1]/input")).SendKeys(keyCom);
            driver.FindElement(By.XPath(string.Format("//*[@id='collapseobj_searchfaq']/tr/td/div[1]/div/table/tbody/tr/td[2]/select/option[{0}]", optionFiltOne))).Click();
            driver.FindElement(By.XPath("//*[@id='collapseobj_searchfaq']/tr/td/div[1]/div/table/tbody/tr/td[3]/select/option[1]")).Click();

            driver.FindElement(By.XPath("//*[@id='collapseobj_searchfaq']/tr/td/div[2]/input[1]")).Click();

            IWebElement elemenTitle = driver.FindElement(By.XPath(xpathTitle));
            
            foreach (var item in arrayCompTitle)
            {

                if (elemenTitle.Text== item)
                {
                    Write(elemenTitle.Text);
                    Assert.Pass();
                }
            }

            Assert.Fail();
        }
      
        [Test]
        public void Help_Srch_1_pos() //TC_Func_01
        {
            Help_Srch(arrayCompareText, arrayCompareTitle, keyWordsArray[0], xpathTitle[0], xpathText[0], optionFilterOne[0]);           
        }

        [Test]
        public void Help_Srch_1_neg() //TC_Func_02
        {
            Help_Srch(arrayCompareTitle, keyWordsArray[1], xpathTitle[1], optionFilterOne[0]);
        }

        [Test]
        public void Help_Srch_2_pos() //TC_Func_04
        {
            Help_Srch(arrayCompareText, arrayCompareTitle, keyWordsArray[5], xpathTitle[3], xpathText[1], optionFilterOne[1]);
        }

        [Test]
        public void Help_Srch_2_neg() //TC_Func_04
        {
            Help_Srch(arrayCompareTitle, keyWordsArray[2], xpathTitle[1], optionFilterOne[1]);
        }
      
        [Test]
        public void Help_Srch_3_pos() //TC_Func_05
        {
            Help_Srch(arrayCompareText, arrayCompareTitle, keyWordsArray[4], xpathTitle[3], xpathText[1], optionFilterOne[2]);
        }

        [Test]
        public void Help_Srch_3_neg() //TC_Func_06
        {
            Help_Srch(arrayCompareTitle, keyWordsArray[3], xpathTitle[1], optionFilterOne[2]);
        }
    
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }     
    }
}