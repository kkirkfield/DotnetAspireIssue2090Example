using Bogus;
using RazorPagesApp.Models;

namespace RazorPagesApp.Data.Fakers;

internal sealed class TodoItemFaker : Faker<TodoItem>
{
    public TodoItemFaker()
    {
        RuleFor(i => i.Id, f => f.Random.Guid());
        RuleFor(i => i.Label, f => f.Lorem.Sentence());
        RuleFor(i => i.Detail, f => f.Lorem.Paragraph().OrNull(f));
        RuleFor(i => i.IsComplete, f => f.Random.Bool());
        RuleFor(i => i.CreatedAt, f => f.Date.PastOffset(refDate: DateTimeOffset.UtcNow));
        RuleFor(i => i.UpdatedAt, f => f.Date.RecentOffset(refDate: DateTimeOffset.UtcNow).OrDefault(f));
        FinishWith((f, i) =>
        {
            if (i.UpdatedAt < i.CreatedAt)
            {
                i.UpdatedAt = i.CreatedAt;
            }
        });
    }
}
