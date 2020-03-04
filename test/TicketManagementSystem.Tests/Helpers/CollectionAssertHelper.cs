using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketManagementSystem.Tests.Helpers
{
    public static class CollectionAssertHelper
    {
        public static void IsEmpty<T>(ICollection<T> collection)
        {
            Assert.IsFalse(collection.Any());
        }

        public static void Count<T>(ICollection<T> collection, int expectedCount)
        {
            Assert.AreEqual(expectedCount, collection.Count);
        }
    }
}
