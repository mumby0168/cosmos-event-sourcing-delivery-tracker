using Convey.CQRS.Commands;
using DeliveryTracker.Application.Infrastructure;

namespace DeliveryTracker.Application.Commands.Stops;

public static class FailStop
{
    public record Command(
        string ScheduleId,
        Guid StopId,
        string Reason) : ICommand;
    
    public class Handler : ICommandHandler<Command>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public Handler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        
        public async Task HandleAsync(Command command, CancellationToken cancellationToken)
        {
            var (scheduleId, stopId, reason) = command;
            
            var schedule = await _scheduleRepository.ReadAsync(scheduleId);

            schedule.FailStop(stopId, reason);

            await _scheduleRepository.SaveAsync(schedule);
        }
    }
}