﻿using MRA.Jobs.Application.IntegrationTests;
using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Trainings;
public class TrainingsContext : CategoryContext
{
    public async Task<TrainingVacancy> GetTraining(string title, DateTime endDate)
    {
        var training = await FindFirstOrDefaultAsync<TrainingVacancy>(t => t.Title == title);
        if (training != null)
            return training;

        var newTraining = new TrainingVacancy
        {
            Title = title,
            Description = "Hello",
            ShortDescription = "Hi",
            PublishDate = DateTime.Now,
            EndDate =endDate,
            CategoryId = await GetCategoryId("trainingVacancy"),
            Duration = 10,
            Fees = 100,
            VacancyQuestions = new List<VacancyQuestion> {
                new VacancyQuestion {
                    Id = Guid.NewGuid(),
                    Question = "What is your English proficiency level?" }
            },
        };
        newTraining.Slug = newTraining.Title.ToLower().Replace(" ", "-");

        await AddAsync(newTraining);

        return newTraining;
    }

}
