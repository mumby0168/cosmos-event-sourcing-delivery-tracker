using Convey.CQRS.Commands;
using DeliveryTracker.Application.Infrastructure;

namespace DeliveryTracker.Application.Commands;

public static class StartSchedule
{
    public record Command(string ScheduleId) : ICommand;
    
    public class Handler : ICommandHandler<Command>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public Handler(IScheduleRepository scheduleRepository) => 
            _scheduleRepository = scheduleRepository;

        public async Task HandleAsync(Command command, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleRepository.ReadAsync(command.ScheduleId);
            
            schedule.Start();

            await _scheduleRepository.SaveAsync(schedule);
        }
    }
}