using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Pages
{
    public abstract class ModalBase
    {
        private readonly InternalTestContext _context;
        protected readonly PageBase _parent;

        protected IWebDriver _driver => _context?.Driver;

        public ModalBase(PageBase parent, InternalTestContext context)
        {
            _parent = parent;
            _context = context;
        }

        #region Elements.

        public IWebElement CloseButton => _driver.FindElement(By.CssSelector(".modal .modal-dialog > div > div.modal-header > button.close[data-dismiss=modal]"));

        #endregion

        #region Methods.

        public void Close()
        {
            CloseButton?.Click();
        }

        #endregion
    }
}
