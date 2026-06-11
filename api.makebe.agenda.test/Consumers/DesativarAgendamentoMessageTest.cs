using FluentAssertions;
using DesativarAgendamentoEvent;

namespace api.makebe.agenda.test.Consumers
{
    public class DesativarAgendamentoMessageTest
    {
        [Fact]
        public void DesativarAgendamentoMessage_DevePermitirDefinirId()
        {
            var message = new DesativarAgendamentoMessage
            {
                Id = 10
            };

            message.Id.Should().Be(10);
        }
    }
}
