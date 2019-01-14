using FluentAssertions;
using LogTreatment;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace XUnitTestProject1
{
    public class MappingTest
    {
        [Fact]
        public void TestMap()
        {
            string url = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";
            Mapping mapping = new Mapping(url);
            string fileLog = mapping.MapMinhaCdnToAgora();

            StringBuilder expectedLog = new StringBuilder();
            expectedLog.AppendLine("#Fields: provider http-method status-code uri-path time-taken response - size cache - status");
            expectedLog.AppendLine(@"""MINHA CDN"" GET 200 /robots.txt 100 312 HIT");
            expectedLog.AppendLine(@"""MINHA CDN"" POST 200 /myImages 319 101 MISS");
            expectedLog.AppendLine(@"""MINHA CDN"" GET 404 /not-found 143 199 MISS");

            string savedLog = File.ReadAllText($".\\{fileLog}");

            savedLog.Should().Contain(expectedLog.ToString(), "The saved file is in the wrong format");
        }

        [Fact]
        public void TestInvalidUrl()
        {
            string url = "test";
            Mapping mapping = new Mapping(url);

            Action action = () => mapping.MapMinhaCdnToAgora();
            action.Should().Throw<Exception>().WithMessage("The url is invalid");
        }

        [Fact]
        public void TestEmtpyUrl()
        {
            string url = string.Empty;
            Mapping mapping = new Mapping(url);

            Action action = () => mapping.MapMinhaCdnToAgora();
            action.Should().Throw<Exception>().WithMessage("The url is empty");
        }
    }
}
