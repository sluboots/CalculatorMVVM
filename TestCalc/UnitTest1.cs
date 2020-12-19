using Calc.Models;
using FluentAssertions;
using NUnit.Framework;

namespace TestCalc
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Add_40and55_95return()
        {
            CalcModel.Calculate("40 + 55").Should().Be("95");
        }
        [Test]
        public void Add_minus35and130_95return()
        {
            CalcModel.Calculate("-35 + 130").Should().Be("95");
        }
        [Test]
        public void Sub_36and26_10return()
        {
            CalcModel.Calculate("36 - 26").Should().Be("10");
        }
        [Test]
        public void Sub_minus2067and45_minus2112return()
        {
            CalcModel.Calculate("-2067 - 45").Should().Be("-2112");
        }
        [Test]
        public void Mult_9and10_90return()
        {
            CalcModel.Calculate("9 * 10").Should().Be("90");
        }
        [Test]
        public void Mult_minus5and0_0return()
        {
            CalcModel.Calculate("5 * 0").Should().Be("0");
        }
    }
}