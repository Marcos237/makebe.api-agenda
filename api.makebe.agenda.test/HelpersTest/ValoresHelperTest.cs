using api.makebe.agenda.domain.Helpers;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace api.makebe.agenda.test.HelpersTest
{
    public class ValoresHelperTest
    {

        [Fact]
        public void ValoresHelper_MontarDate_DeveRetornarDataCorreta()
        {
            var response = ValoresHelper.MontarDate("10/10/2025 09:00:00", "10/03/2025");

            response.Should().HaveValue();
            response!.Value.Should().Be(10.March(2025).At(9, 0, 0));
        }
    }
}
