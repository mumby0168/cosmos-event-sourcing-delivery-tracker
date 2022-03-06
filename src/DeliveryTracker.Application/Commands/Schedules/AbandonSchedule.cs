using Convey.CQRS.Commands;
using DeliveryTracker.Application.Infrastructure;

namespace DeliveryTracker.Application.Commands.Schedules;

public static class AbandonSchedule
{
    public record Command(
        string ScheduleId,
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
            var (scheduleId, reason) = command;
            
            var schedule = await _scheduleRepository.ReadAsync(scheduleId);

            schedule.Abandon(reason);

            await _scheduleRepository.SaveAsync(schedule);
        }
    }
}