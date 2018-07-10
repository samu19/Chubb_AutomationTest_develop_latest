using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellerDetailsPageAutomation
{
    public class EditTravellerDetailsPage
    {
        public static EditTravellerDetailsCommand FillSection(IFillable sectionInfo)
        {
            return new EditTravellerDetailsCommand(sectionInfo);
        }
    }

    public class EditTravellerDetailsCommand
    {
        private List<IFillable> sectionsToFill;

        public EditTravellerDetailsCommand(IFillable sectionInfo)
        {
            sectionsToFill = new List<IFillable>();
            sectionsToFill.Add(sectionInfo);
        }

        public EditTravellerDetailsCommand FillSection(IFillable sectionInfo)
        {
            this.sectionsToFill.Add(sectionInfo);
            return this;
        }

        public void Proceed(FullElementSelector fullElementSelector)
        {
            foreach (IFillable section in this.sectionsToFill)
            {
                section.Fill(fullElementSelector);

            }

            // Trigger Proceed Button Here /html/body/chubb-dbs-app/app-summary/app-traveller-detail/form/div[4]/div/div[2]/button
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-summary/app-traveller-detail/form/div[4]/div/div[2]/button")));

            Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-summary/app-traveller-detail/form/div[4]/div/div[2]/button")).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("summary/preview"));
        }


    }
}
