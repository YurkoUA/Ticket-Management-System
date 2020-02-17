using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Pages.Ticket.Modal
{
    public class TicketSearchModal : ModalBase
    {
        public TicketSearchModal(PageBase parent, InternalTestContext context) : base(parent, context)
        {
        }

        #region Elements.

        public IWebElement NumberField => _driver.FindElement(By.CssSelector("#form0 input.form-control[type=number][name-number]"));

        public IWebElement SearchButton => _driver.FindElement(By.CssSelector("#form0 input.btn.btn-primary.btn-loading[type=submit]"));

        public IWebElement ResultsContainer => _driver.FindElement(By.Id("search-result"));

        #endregion

        #region Methods.

        public void Search(string number)
        {
            NumberField.Clear();
            NumberField.SendKeys(number);
            SearchButton.Click();
        }

        #endregion

        #region Asserters.



        #endregion
    }
}
