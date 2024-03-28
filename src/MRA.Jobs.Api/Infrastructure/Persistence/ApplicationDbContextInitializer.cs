using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext, IConfiguration configuration)
{
    public async Task SeedAsync()
    {
        await CreateNoVacancy("NoVacancy");

        if (configuration["Environment"] == "Production")
            return;

        await CreateJobsVacancyAsync();
        await CreateInternshipsVacancyAsync();
        await CreateTrainingsVacancyAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateNoVacancy(string vacancyTitle)
    {
        var noCategory =
            await dbContext.Categories.FirstOrDefaultAsync(c => c.Slug == CommonVacanciesSlugs.NoVacancySlug);
        if (noCategory == null)
        {
            var category = new VacancyCategory { Name = "NoVacancy", Slug = CommonVacanciesSlugs.NoVacancySlug };
            await dbContext.Categories.AddAsync(category);
            noCategory = category;
        }

        List<VacancyQuestion> question = new()
        {
            new VacancyQuestion { Question = "Your name", IsOptional = false },
            new VacancyQuestion { Question = "Your phone number", IsOptional = false }
        };

        var noVacancy = await dbContext.JobVacancies
            .Include(vacancy => vacancy.VacancyQuestions)
            .FirstOrDefaultAsync(hv => hv.Slug == CommonVacanciesSlugs.NoVacancySlug);
        if (noVacancy == null)
        {
            var vacancy = new JobVacancy
            {
                Title = vacancyTitle,
                PublishDate = DateTime.Now,
                EndDate = new DateTime(2099, 12, 31),
                ShortDescription = "",
                Description = "",
                Slug = CommonVacanciesSlugs.NoVacancySlug,
                CategoryId = noCategory.Id,
                CreatedAt = DateTime.Now,
                CreatedByEmail = "mrajobsadmin@silkroadprofessionals.com",
                VacancyQuestions = question
            };

            await dbContext.JobVacancies.AddAsync(vacancy);
        }

        if (noVacancy != null && !noVacancy.VacancyQuestions.Any())
            noVacancy.VacancyQuestions = question;

        await dbContext.SaveChangesAsync();
    }

    private async Task<VacancyCategory> CreateVacancyCategoryAsync(string name, string slug)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Slug == slug && x.Name == name);
        if (category != null)
            return category;

        category = new VacancyCategory() { Name = name, Slug = slug };
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();
        return category;
    }

    private async Task CreateJobsVacancyAsync()
    {
        var category = await CreateVacancyCategoryAsync("IT Specialists", "it-specialists");

        if (await dbContext.JobVacancies.FirstOrDefaultAsync(x => x.Slug == "devops-engineer") == null)
            await dbContext.JobVacancies.AddAsync(new JobVacancy
            {
                Slug = "devops-engineer",
                Title = "DevOps Engineer",
                ShortDescription =
                    "We need a lion, a tiger, an organizer by life, a winner by nature, in short - a DevOps-engineer.",
                Description =
                    "Не просто DevOps-инженер, а человек всесторонний, который с лёгкостью найдёт проблему и решит её, мыслит наперёд и предсказывает ошибки в будущем.\n\nУзнали в описании себя? Подайте заявку и станьте частью крутой финтех команды — Alif\n\nТребования:\n\nЗнания Linux (CentOS, Debian);\nПонимание работы с Linux в целом: настройка сети, устройство ФС, права доступа;\nУверенное знание командной строки и базовых утилит;\nУмение развернуть готовый стек (Python, PHP, Ruby, DB) для работы в production;\nУмение писать скрипты (sh, bash);\nMySQL, опыт написания простых запросов;\nБазовые знания docker.\nБудет плюсом:\n\nАnsible;\nGitLab;\nЗнание любой из систем мониторинга: zabbix, prometheus и сопутствующих утилит;\nОпыт работы с брокерами сообщений.\nДля нас самое ценное:\n\nЧестность и скромность;\nОтветственность и пунктуальность;\nУсердие в саморазвитии и в работе.\nПредлагаем:\n\nКарьерный рост;\nДружелюбный коллектив;\nКомфортный офис;\nВозможность развития вместе с компанией.\nЕсли Вы хотите участвовать в интересных проектах, работать в комфортных условиях и готовы стать частью нашей команды - ждем ваших резюме!",
                PublishDate = DateTime.Today.AddDays(-10),
                EndDate = DateTime.Today.AddDays(30),
                CategoryId = category.Id,
                WorkSchedule = WorkSchedule.FullTime,
                RequiredYearOfExperience = 3,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "Опишите ваш опыт работы с Linux (CentOS, Debian)." },
                    new VacancyQuestion { Question = "Расскажите о вашем опыте написания скриптов (sh, bash)." },
                    new VacancyQuestion { Question = "Как вы обычно настраиваете сеть и устройства ФС в Linux?" },
                    new VacancyQuestion { Question = "Какой стек вы обычно разворачиваете для работы в production?" },
                    new VacancyQuestion
                    {
                        Question =
                            "Опишите ваш опыт работы с MySQL и приведите примеры простых запросов, которые вы написали.",
                        IsOptional = true
                    },
                    new VacancyQuestion
                    {
                        Question = "Есть ли у вас опыт работы с Docker? Если да, расскажите о нем.",
                        IsOptional = true
                    }
                ]
            });

        if (await dbContext.JobVacancies.FirstOrDefaultAsync(x => x.Slug == "backend-developer") == null)
            await dbContext.JobVacancies.AddAsync(new JobVacancy
            {
                Slug = "backend-developer",
                Title = "Backend Developer",
                ShortDescription = "We are looking for a skilled Backend Developer to join our team.",
                Description =
                    "As a Backend Developer, you will be responsible for server-side web application logic and integration of the front-end part...",
                PublishDate = DateTime.Today.AddDays(-15),
                EndDate = DateTime.Today.AddDays(20),
                CategoryId = category.Id,
                WorkSchedule = WorkSchedule.FullTime,
                RequiredYearOfExperience = 2,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What programming languages are you proficient in?" },
                    new VacancyQuestion { Question = "Describe a project where you used microservices." },
                    new VacancyQuestion
                    {
                        Question = "How do you handle error handling and debugging when programming?",
                        IsOptional = true
                    }
                ],
                VacancyTasks =
                [
                    new VacancyTask() { Title = "", Description = "", Template = "", Test = "" }
                ],
                Tags =
                [
                    new VacancyTag { Tag = new Tag { Name = "DotNet" } },
                    new VacancyTag { Tag = new Tag { Name = "developer" } }
                ]
            });

        if (await dbContext.JobVacancies.FirstOrDefaultAsync(x => x.Slug == "frontend-developer") == null)
            await dbContext.JobVacancies.AddAsync(new JobVacancy
            {
                Slug = "frontend-developer",
                Title = "Frontend Developer",
                ShortDescription = "We are seeking a driven Frontend Developer to join our growing team.",
                Description =
                    "In this role, you will be responsible for developing and implementing user interface components...",
                PublishDate = DateTime.Today.AddDays(-30),
                EndDate = DateTime.Today.AddDays(-1),
                CategoryId = category.Id,
                WorkSchedule = WorkSchedule.FullTime,
                RequiredYearOfExperience = 2,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What JavaScript frameworks have you used?" },
                    new VacancyQuestion
                    {
                        Question = "How do you ensure browser compatibility in your front-end applications?"
                    },
                    new VacancyQuestion
                    {
                        Question = "Describe a time when you had to optimize a web application for performance.",
                        IsOptional = true
                    }
                ]
            });


        await dbContext.SaveChangesAsync();
    }

    private async Task CreateInternshipsVacancyAsync()
    {
        var category = await CreateVacancyCategoryAsync("IT Specialists", "it-specialists");

        if (await dbContext.Internships.FirstOrDefaultAsync(x => x.Slug == "data-scientist-intern") == null)
            await dbContext.Internships.AddAsync(new InternshipVacancy()
            {
                Slug = "data-scientist-intern",
                Title = "Data Scientist Intern",
                ShortDescription = "We are seeking a motivated Data Scientist Intern to join our team.",
                Description =
                    "As a Data Scientist Intern, you will be responsible for analyzing large, complex datasets and using machine learning algorithms to help us make strategic decisions...",
                PublishDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(90),
                CategoryId = category.Id,
                ApplicationDeadline = DateTime.Today.AddDays(30),
                Duration = 3,
                Stipend = 15,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What programming languages are you proficient in?" },
                    new VacancyQuestion { Question = "Describe a project where you used machine learning algorithms." },
                    new VacancyQuestion
                    {
                        Question = "How do you handle data cleaning and preprocessing?", IsOptional = true
                    }
                ]
            });

        if (await dbContext.Internships.FirstOrDefaultAsync(x => x.Slug == "frontend-developer-intern") == null)
            await dbContext.Internships.AddAsync(new InternshipVacancy()
            {
                Slug = "frontend-developer-intern",
                Title = "Frontend Developer Intern",
                ShortDescription = "We are seeking a motivated Frontend Developer Intern to join our team.",
                Description =
                    "As a Frontend Developer Intern, you will be responsible for developing and implementing user interface components...",
                PublishDate = DateTime.Today.AddDays(-30),
                EndDate = DateTime.Today.AddDays(60),
                CategoryId = category.Id,
                ApplicationDeadline = DateTime.Today.AddDays(-5),
                Duration = 1,
                Stipend = 15,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What programming languages are you proficient in?" },
                    new VacancyQuestion { Question = "Describe a project where you used React.js or Vue.js." },
                    new VacancyQuestion { Question = "How do you handle responsive design?", IsOptional = true }
                ]
            });

        if (await dbContext.Internships.FirstOrDefaultAsync(x => x.Slug == "full-stack-developer-intern") == null)
            await dbContext.Internships.AddAsync(new InternshipVacancy()
            {
                Slug = "full-stack-developer-intern",
                Title = "Full Stack Developer Intern",
                ShortDescription = "We are seeking a motivated Full Stack Developer Intern to join our team.",
                Description =
                    "As a Full Stack Developer Intern, you will be responsible for developing both frontend and backend parts of web applications...",
                PublishDate = DateTime.Today.AddDays(-90),
                EndDate = DateTime.Today.AddDays(-10),
                CategoryId = category.Id,
                ApplicationDeadline = DateTime.Today.AddDays(-60),
                Duration = 3,
                Stipend = 15,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What programming languages are you proficient in?" },
                    new VacancyQuestion
                    {
                        Question = "Describe a project where you worked on both frontend and backend."
                    },
                    new VacancyQuestion { Question = "How do you handle database management?", IsOptional = true }
                ]
            });

        await dbContext.SaveChangesAsync();
    }

    private async Task CreateTrainingsVacancyAsync()
    {
        var category = await CreateVacancyCategoryAsync("IT Specialists", "it-specialists");

        if (await dbContext.TrainingVacancies.FirstOrDefaultAsync(x => x.Slug == "frontend-developer-training") == null)
            await dbContext.TrainingVacancies.AddAsync(new TrainingVacancy()
            {
                Slug = "frontend-developer-training",
                Title = "Frontend Developer Training",
                ShortDescription = "We are seeking motivated individuals for our Frontend Developer Training.",
                Description =
                    "In this training, you will learn about developing and implementing user interface components...",
                PublishDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(1000),
                CategoryId = category.Id,
                Duration = 120,
                Fees = 2,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What programming languages are you proficient in?" },
                    new VacancyQuestion { Question = "Describe a project where you used React.js or Vue.js." },
                    new VacancyQuestion { Question = "How do you handle responsive design?", IsOptional = true }
                ]
            });

        if (await dbContext.TrainingVacancies.FirstOrDefaultAsync(x => x.Slug == "full-stack-developer-training") ==
            null)
            await dbContext.TrainingVacancies.AddAsync(new TrainingVacancy()
            {
                Slug = "full-stack-developer-training",
                Title = "Full Stack Developer Training",
                ShortDescription = "We are seeking motivated individuals for our Full Stack Developer Training.",
                Description =
                    "In this training, you will learn about developing both frontend and backend parts of web applications...",
                PublishDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(1000),
                CategoryId = category.Id,
                Duration = 120,
                Fees = 3,
                VacancyQuestions =
                [
                    new VacancyQuestion { Question = "What programming languages are you proficient in?" },
                    new VacancyQuestion
                    {
                        Question = "Describe a project where you worked on both frontend and backend."
                    },
                    new VacancyQuestion { Question = "How do you handle database management?", IsOptional = true }
                ]
            });
        await dbContext.SaveChangesAsync();
    }

    private async Task CreateApplicationsAsync()
    {
        var vacancy = await dbContext.JobVacancies
            .Include(x => x.VacancyQuestions)
            .Include(x => x.VacancyTasks)
            .FirstOrDefaultAsync(x => x.Slug == "backend-developer");

        if (vacancy == null) return;

        if (await dbContext.Applications
                .FirstOrDefaultAsync(x => x.Slug == "applicant1-backend-developer") != null) return;

        var application = new Domain.Entities.Application()
        {
            VacancyId = vacancy.Id,
            CoverLetter = "test test test",
            AppliedAt = DateTime.Now,
            VacancyResponses = vacancy.VacancyQuestions.Select(vacancyQuestion =>
                new VacancyResponse { VacancyQuestion = vacancyQuestion, Response = "test test test" }).ToList(),
            ApplicantUsername = "applicant1",
            TaskResponses = vacancy.VacancyTasks.Select(vacancyTask =>
                new TaskResponse { TaksId = vacancyTask.Id, Code = "Console.WriteLine('Hello World!')" }).ToList(),
            Slug = "applicant1-backend-developer",
            CV = "cv.pdf"
        };
        await dbContext.Applications.AddAsync(application);
        await dbContext.ApplicationTimelineEvents.AddAsync(new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            Note = "Applied",
            CreateBy = "applicant1",
            EventType = TimelineEventType.Created,
            Time = DateTime.Now,
            
        });

        await dbContext.SaveChangesAsync();
    }
}