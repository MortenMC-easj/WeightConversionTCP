using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeightConversion.Library.Test
{
    [TestClass]
    public class UnitTest1
    {
        //Testing the Converters
        [TestMethod]
        public void EqualConvertToGram()
        {
           Assert.AreEqual(85.04856 , WeightConverter.ConvertToGram(3), 0.001);
        }

        [TestMethod]
        public void EqualConvertToOunce()
        {
            Assert.AreEqual(3.527396195, WeightConverter.ConvertToOunce(100), 0.001);
        }
        

       

        


    }
}
