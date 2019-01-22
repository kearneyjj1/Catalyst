using System.Collections.Generic;
using System.Text;
using Catalyst.Helpers.Hex.HexConverters.Extensions;
using Xunit;

namespace Catalyst.Helpers.RLP.UnitTests
{
    public class RLPTests
    {
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="expected"></param>
        private static void AssertStringEncoding(string test, string expected)
        {
            var testBytes = test.ToBytesForRlpEncoding();
            var encoderesult = RLP.EncodeElement(testBytes);
            Assert.Equal(expected, encoderesult.ToHex());

            var decodeResult = RLP.Decode(encoderesult)[0].RlpData;
            Assert.Equal(test, decodeResult.ToStringFromRlpDecoded());
        }

        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="expected"></param>
        private static void AssertIntEncoding(int test, string expected)
        {
            var testBytes = test.ToBytesForRlpEncoding();
            var encoderesult = RLP.EncodeElement(testBytes);
            Assert.Equal(expected, encoderesult.ToHex());

            var decodeResult = RLP.Decode(encoderesult)[0].RlpData;
            Assert.Equal(test, decodeResult.ToBigIntegerFromRlpDecoded());
        }

        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="expected"></param>
        private static void AssertStringCollection(string[] test, string expected)
        {
            var encoderesult = RLP.EncodeList(EncodeElementsBytes(test.ToBytesForRlpEncoding()));
            Assert.Equal(expected, encoderesult.ToHex());

            var decodeResult = RLP.Decode(encoderesult)[0] as RlpCollection;
            for (var i = 0; i < test.Length; i++)
                Assert.Equal(test[i], decodeResult[i].RlpData.ToStringFromRlpDecoded());
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static byte[][] EncodeElementsBytes(params byte[][] bytes)
        {
            var encodeElements = new List<byte[]>();
            foreach (var byteElement in bytes)
                encodeElements.Add(RLP.EncodeElement(byteElement));
            return encodeElements.ToArray();
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeBigInteger()
        {
            var test = "100102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f".HexToByteArray()
                .ToBigIntegerFromRlpDecoded();
            var expected = "a0100102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f";
            var testBytes = test.ToBytesForRlpEncoding();
            var encoderesult = RLP.EncodeElement(testBytes);
            Assert.Equal(expected, encoderesult.ToHex());

            var decodeResult = RLP.Decode(encoderesult)[0].RlpData;
            Assert.Equal(test, decodeResult.ToBigIntegerFromRlpDecoded());
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeEmptyList()
        {
            var test = new byte[0][];
            var expected = "c0";
            var encoderesult = RLP.EncodeList(test);
            Assert.Equal(expected, encoderesult.ToHex());

            var decodeResult = RLP.Decode(encoderesult)[0] as RlpCollection;
            Assert.True(decodeResult.Count == 0);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeEmptyString()
        {
            var test = "";
            var testBytes = Encoding.UTF8.GetBytes(test);
            var expected = "80";
            var encoderesult = RLP.EncodeElement(testBytes);
            Assert.Equal(expected, encoderesult.ToHex());
            var decodeResult = RLP.Decode(encoderesult)[0].RlpData;
            Assert.Null(decodeResult);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeLongString()
        {
            var test = "Lorem ipsum dolor sit amet, consectetur adipisicing elit"; // length = 56
            var expected =
                "b8384c6f72656d20697073756d20646f6c6f722073697420616d65742c20636f6e7365637465747572206164697069736963696e6720656c6974";
            AssertStringEncoding(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeLongStringList()
        {
            var element1 = "cat";
            var element2 = "Lorem ipsum dolor sit amet, consectetur adipisicing elit";
            string[] test = {element1, element2};
            var expected =
                "f83e83636174b8384c6f72656d20697073756d20646f6c6f722073697420616d65742c20636f6e7365637465747572206164697069736963696e6720656c6974";
            AssertStringCollection(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeMediumInteger()
        {
            var test = 1000;
            var expected = "8203e8";

            AssertIntEncoding(test, expected);
            test = 1024;
            expected = "820400";
            AssertIntEncoding(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeShortString()
        {
            var test = "dog";
            var expected = "83646f67";
            AssertStringEncoding(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeShortStringList()
        {
            string[] test = {"cat", "dog"};
            var expected = "c88363617483646f67";

            AssertStringCollection(test, expected);
            test = new[] {"dog", "god", "cat"};
            expected = "cc83646f6783676f6483636174";
            AssertStringCollection(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeSingleCharacter()
        {
            var test = "d";
            var expected = "64";
            AssertStringEncoding(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeSmallInteger()
        {
            var test = 15;
            var expected = "0f";
            AssertIntEncoding(test, expected);
        }

        /// <summary>
        /// </summary>
        [Fact]
        public void ShouldEncodeZeroAs80()
        {
            var test = 0;
            var expected = "80";
            AssertIntEncoding(test, expected);
        }
    }
}