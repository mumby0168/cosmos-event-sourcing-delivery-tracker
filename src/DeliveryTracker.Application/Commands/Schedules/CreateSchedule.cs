using Convey.CQRS.Commands;
using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Domain.Aggregates;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Application.Commands.Schedules;

public static class CreateSchedule
{
    public record Command(
        string DriverCode,
        string DriverFirstName,
        string DriverSecondName) : ICommand;

    public class Handler : ICommandHandler<Command>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public Handler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task HandleAsync(Command command, CancellationToken cancellationToken)
        {
            var (driverCode, driverFirstName, driverSecondName) = command;
            
            var schedule = new Schedule(new Driver(
                driverCode,
                driverFirstName,
                driverSecondName));

            await _scheduleRepository.SaveAsync(schedule);
        }
    }
}