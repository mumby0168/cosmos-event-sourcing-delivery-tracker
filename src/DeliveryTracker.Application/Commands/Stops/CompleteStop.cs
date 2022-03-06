using Convey.CQRS.Commands;
using DeliveryTracker.Application.Infrastructure;

namespace DeliveryTracker.Application.Commands.Stops;

public static class CompleteStop
{
    public record Command(
        string ScheduleId,
        Guid StopId) : ICommand;

    public class Handler : ICommandHandler<Command>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public Handler(IScheduleRepository scheduleRepository) => 
            _scheduleRepository = scheduleRepository;

        public async Task HandleAsync(Command command, CancellationToken cancellationToken)
        {
            var (scheduleId, stopId) = command;
            
            var schedule = await _scheduleRepository.ReadAsync(scheduleId);

            schedule.CompleteStop(stopId);

            await _scheduleRepository.SaveAsync(schedule);
        }
    }
}