using Convey.CQRS.Commands;
using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Application.Commands.Stops;

public static class ScheduleStop
{
    public record Command(
        string ScheduleId,
        int HouseNumber,
        string AddressLine,
        string PostCode) : ICommand;

    public class Handler : ICommandHandler<Command>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public Handler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        
        public async Task HandleAsync(Command command, CancellationToken cancellationToken)
        {
            var (scheduleId, houseNumber, addressLine, postCode) = command;
            
            var schedule = await _scheduleRepository.ReadAsync(scheduleId);

            schedule.AddStop(new Location(
                houseNumber, 
                addressLine, 
                postCode));

            await _scheduleRepository.SaveAsync(schedule);
        }
    }
}