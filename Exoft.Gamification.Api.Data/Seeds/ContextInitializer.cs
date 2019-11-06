using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Exoft.Gamification.Api.Data.Seeds
{
    public class ContextInitializer
    {
        public static void Initialize(UsersDbContext context)
        {
            context.Database.EnsureCreated();

            var user1 = new Core.Entities.User()
            {
                FirstName = "Ostap",
                LastName = "Roik",
                Email = "ostap2308@gmail.com",
                UserName = "OstapRoik",
                Password = "804f50ddbaab7f28c933a95c162d019acbf96afde56dba10e4c7dfcfe453dec4bacf5e78b1ddbdc1695a793bcb5d7d409425db4cc3370e71c4965e4ef992e8c4",
                // Password123
                Status = "Status bla bla bla",
                XP = 30
            };
            var user2 = new Core.Entities.User()
            {
                FirstName = "Tanya",
                LastName = "Gogina",
                Email = "tanyagermain23@gmail.com",
                UserName = "TanyaGogina",
                Password = "804f50ddbaab7f28c933a95c162d019acbf96afde56dba10e4c7dfcfe453dec4bacf5e78b1ddbdc1695a793bcb5d7d409425db4cc3370e71c4965e4ef992e8c4",
                // Password123
                Status = "Status 123",
                XP = 40
            };
            var user3 = new Core.Entities.User()
            {
                FirstName = "Oleksandr",
                LastName = "Brychkin",
                Email = "obrychkin@exoft.net",
                UserName = "OleksandrBrychkin",
                Password = "804f50ddbaab7f28c933a95c162d019acbf96afde56dba10e4c7dfcfe453dec4bacf5e78b1ddbdc1695a793bcb5d7d409425db4cc3370e71c4965e4ef992e8c4",
                // Password123
                Status = null,
                XP = 10
            };
            var achievement1 = new Core.Entities.Achievement()
            {
                Name = "Welcome",
                Description = "A newcomer to the team",
                XP = 10
            };
            var achievement2 = new Core.Entities.Achievement()
            {
                Name = "1 year",
                Description = "1 year in company",
                XP = 30
            };
            var role1 = new Core.Entities.Role()
            {
                Name = "Admin"
            };
            var role2 = new Core.Entities.Role()
            {
                Name = "User"
            };
            var event1 = new Core.Entities.Event()
            {
                Description = "First",
                Type = GamificationEnums.EventType.Race,
                User = user1
            };
            var event2 = new Core.Entities.Event()
            {
                Description = "Second",
                Type = GamificationEnums.EventType.Records,
                User = user1
            };
            var event3 = new Core.Entities.Event()
            {
                Description = "Third",
                Type = GamificationEnums.EventType.Upload,
                User = user3
            };

            if (!context.Users.Any())
            {
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.Users.Add(user3);
                context.SaveChanges();
            }

            if (!context.Achievements.Any())
            {
                context.Achievements.Add(achievement1);
                context.Achievements.Add(achievement2);
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                context.Roles.Add(role1);
                context.Roles.Add(role2);
                context.SaveChanges();
            }

            if (!context.UserAchievement.Any())
            {
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user1,
                    Achievement = achievement2
                });
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user2,
                    Achievement = achievement1
                });
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user2,
                    Achievement = achievement2
                });
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user3,
                    Achievement = achievement1
                });
                context.SaveChanges();
            }

            if (!context.UserRoles.Any())
            {
                context.UserRoles.Add(new Core.Entities.UserRoles()
                {
                    User = user1,
                    Role = role2
                });
                context.UserRoles.Add(new Core.Entities.UserRoles()
                {
                    User = user2,
                    Role = role2
                });
                context.UserRoles.Add(new Core.Entities.UserRoles()
                {
                    User = user2,
                    Role = role1
                });
                context.UserRoles.Add(new Core.Entities.UserRoles()
                {
                    User = user3,
                    Role = role1
                });
                context.SaveChanges();
            }

            if (!context.Events.Any())
            {
                context.Events.Add(event1);
                context.Events.Add(event2);
                context.Events.Add(event3);
                context.SaveChanges();
            }

            if (!context.Chapters.Any())
            {
                context.Chapters.AddRange(chapters);
                context.SaveChanges();
            }
        }

        private static List<Chapter> chapters = new List<Chapter>
        {
            new Chapter
            {
                Title = "Introduction",
                OrderId = 1,
                Articles = new List<Article>
                {
                    new Article { Title = "", Text = "We are glad to welcome you in our company! We are happy that you have chosen Exoft and hope, that our cooperation will be long lasting and productive." }
                }
            },
            new Chapter
            {
                Title = "About us",
                OrderId = 2,
                Articles = new List<Article>
                {
                    new Article { Title = "", Text = @"Exoft was founded in 2013 in Lviv, Ukraine as a customer development provider specializing in development using Microsoft technologies.
Our main specialization is web, desktop and mobile development using following technologies: C#/.NET, ASP .NET MVC, Web API, WPF, Xamarin, AngularJS.
What about Exoft’s logo? A few years ago our guys were thinking about how to name their startup. They wanted to express their feelings and give the understanding of what they do. Our logo means Excellent Software and our goal is to make it really excellent.
New employees should get acquainted with the reference book along with work contract. This will help to get a closer look at working and business processes in the Company. The present reference book will serve as a guidance for quick introduction to corporate rules and culture of the Company.
All rules described here have mandatory nature and should be followed by all employees. In the process of Company’s growth and development some rules can be changed and modified. Besides, if you have a good advice or suggestion - we will be happy if you share with us. Exoft is always open for good ideas.
Furthermore, if you have any questions or difficulties please approach HR department." }
                }
            },
            new Chapter
            {
                Title = "Chapter 1",
                OrderId = 3,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 1.1, Title = "The beginning of our cooperation", Text = @"Our working relations start the minute you enter the office on your first working day. There is a trial period at Exoft and it lasts 3 months. During this period new employee has to prove his/her professional level, demonstrate knowledge on practice and follow corporate rules and procedures." },
                    new Article { UnitNumber = 1.2, Title = "Working hours and schedule", Text = @"At Exoft we have 5-day working week and 8-hours working day. We practice flexible schedule, though employees should remember about the norm (number of working days in a month multiplied by 8). One should remember that in the end of the working day it is necessary to report hours. There is no definite time to start your work, but workday is to be started before 12 pm. There is a penalty for being late – 50 UAH (this will become a part of general moneybox). 
Extra hours are paid only if they were agreed in advance with authorities and there was a necessity in it. In general, there is no strict policy concerning overworking.
As worked hours we count only those hours, which were spent for work. Dinner time, English lessons, time spent gaming etc. is not working time. On the other hand, time spent outside the office, but while carrying out duties delivered by Exoft can be reported as working hours." },
                    new Article { UnitNumber = 1.3, Title = "Entry meeting", Text = "HR offers an introduction excursion around the office for each new employee, introduces to colleagues, provides information about working processes and follows to workplace." },
                    new Article { UnitNumber = 1.4, Title = "Communication within the Company", Text = @"It is important for the Company to be sure that each employee has all the necessary information regarding working processes at Exoft, and that working atmosphere is comfortable and favorable for work. 
We are trying to provide as much information about changes and decisions as it is possible, as well as the motives and reasons of these changes.
We stick to the policy of “open doors”, consequently all employees willing to discuss their ideas are welcome to do it along with their considerations concerning particular questions or making some reasonable remarks. 
As every employee is an important part of the Company, we are trying to be attentive to everyone and express a deserved appreciation. All employees are equal and valuable; everyone has his or her small mission and together we have one big general goal. That is why we find it important for employees to respect and appreciate colleagues and all the people, whom they are being in contact while performing their duties." },
                    new Article { UnitNumber = 1.5, Title = "Personal possessions", Text = "The Company is not liable for personal property of employees, so please be careful when you leave your possessions." },
                    new Article { UnitNumber = 1.6, Title = "Company property", Text = @"All property which belongs to the Company is to be used by employees. Technical and other equipment is given to ensure effectiveness of work performed and help people to carry out their working duties and should be used only with this purpose, it should be returned on demand.
Things that were given for personal usage to employees, such as mobile phones, lap top computers etc. are to be stored in safe places. It is forbidden to take them out from the office or appropriate it. As an exception you can take the laptop to home, under your own responsibility.
If employee has lost or damaged Company’s property, which was given to his/her usage and it was caused by inattentiveness and thoughtless of employee, the Company can take disciplinary measures."},
                    new Article { UnitNumber = 1.7, Title = "Personal information", Text = @"Personal information is any information, which is stored in computer systems or in paper folders, which by itself or together with any other information about employee allows the Company to identify him or her. Personal information which Exoft can receive about an employee can be the following: name and address, contact information, date of birth, education and qualification, bank account information, salary information, work analysis, workshop attendance tracking and plans of professional improvement for future, etc.
The Company can provide your personal information to third parties (such as pension fund, health care institutions etc.) with the purpose of ensuring necessary legal processes. However, the Company will not give away such information unless it has your agreement, or if other is not prescribed by law." }
                }
            },
            new Chapter
            {
                Title = "Chapter 2",
                OrderId = 4,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 2.1, Title = "Salary", Text = @"Exoft is a competitive company and is trying to keep up with market offer, but we also evaluate self - development, efforts and professional approach of employees.
Salary review for technical (producting)  stuff takes place each 6 months. 
Performance Evaluation for Productive team members consist of 3 stages:
 
1.	Questionnaire before Performance Evaluation meeting (self-assessment) 
2.	Tech interview with tech specialist + Goals for next review period (40 min)
3.	HR interview (discussing feedback from HR, PM, Customer, and team members) + Goals for next review period (30 min)

For administrative personnel it happens in individual order (every 6-12 months).

Performance Evaluation for Administrative team members consist of 2 stages:
 
1.	Self- assessment
2.	HR interview (discussing feedback from HR and team members) + Goals for next review period (30 min)
After all of that HR announce results to the employee in a few days.

 There is no definite amount of money by which the salary can be raised. Also salary review does not mean salary raise, this depends on quality and performance of the employee. 
Furthermore, when employee reaches certain professional level and his/her salary equals $2000 and more, salary review takes place once in a year.
Exoft can also gratify certain employees and their achievements with financial bonus." }
                }
            },
            new Chapter
            {
                Title = "Chapter 3",
                OrderId = 5,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 3.1, Title = "Paid Vacation", Text = @"For each month that you have worked in the company you are entitled to 1,5 day of paid vacation, these are 18 working days in a year. The company tries to practice it’s loyalty and flexibility in relation to employees, that is why one does not have to work for half of the year to be able to take vacation days. 
If you are going to take 3 or more days of vacation, you have to request it in https://exoft.calamari.io in a month before. If you do not request vacation in advance, it can be declined. 
National holidays are day offs, but paid as a working day at Exoft. 

If you have not used vacation days during the base year, they are burned (сanceled) and not compensated in cash equivalent. As an exception, it is possible to transfer to the next base year up to 8 days of unused vacation from the previous year. The base year is the year from the first day of work +365 (366) days.
If you have decided to leave the Company, depending on the situation you can be offered to take vacation days which are left or the Company can pay for these days along with your last salary. If at this point employee has used more vacation days than he/she was entitled to then payment for these hours is being taken from the last salary." },
                    new Article { UnitNumber = 3.2, Title = "Sick leave", Text = @"If you are not feeling well it is better to visit a doctor. You should immediately inform human resource manager, explain the reasons of your absence and possible duration of it.
Please, inform HR manager about your absence personally, not through colleagues and request sick leave here https://exoft.calamari.io   
Sick leave is compensated by the Company, on condition that you have medical certificate. The company compensates 10 days of sick leave per base year. The base year is the year from the first day of work +365 (366) days.
In case you feel sick at workplace, we have medicine box with some necessary medicine aid." },
                    new Article { UnitNumber = 3.3, Title = "Unpaid leave (Days off)", Text = "Day off is a day off from work which is not being paid for. In Exoft you are allowed to take day off only when it is really necessary for you and you have reasonable grounds for it.  You should request your day off in https://exoft.calamari.io The Company still reserves the right to decline your request about day off." },
                    new Article { UnitNumber = 3.4, Title = "Remote work (working from home)", Text = @"Exoft company offers comfortable office, which is equipped with everything necessary for productive and effective work. That is why we do not see the necessity to work from home. However, working from home is allowed in individual cases under the condition that employee is currently engaged in the project and will provide detailed report for worked hours. This should be arranged with HR and colleagues on the project and requested at https://exoft.calamari.io  Working from home is an exception rather than a rule, so you can work from home for 1-2 days, if you see that it will take longer time please take vacation or days off." }
                }
            },
            new Chapter
            {
                Title = "Chapter 4",
                OrderId = 6,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 4.1, Title = "Rules and norms", Text = @"Communication with head managers, colleagues and customers, professional performance of one’s duties and following the workday discipline – are the keys which form norms of corporate behavior at Exoft.
Norms and rules are common for everyone. It is essentially important for each employee to follow corporate culture and rules accepted at Exoft.
Values define the culture of our Company. Everything what is being done in  Exoft is done on the highest level and we never save on quality. 
The Company strives to satisfy customers’ expectations and fulfil their needs. Employees in Exoft are responsible for their words, actions and results of their work individually and as a part of the team. Exoft team operates in accordance with corporate values and we measure our success by success of customers and people, whom we are working with. General norms:
•	Responsibility and reliability;
•	Confidentiality;
•	Objectiveness and independency;
•	Professionality and the highest quality;
•	Respect;
•	Decency and integrity;
•	Responsibility for each performed task;
•	Professional approach towards work." },
                    new Article { UnitNumber = 4.2, Title = "Comfort in the open space", Text = @"Please respect the comfort of your colleagues and adhere to certain rules of conduct in the open space:
•	Do not shout and  laugh loudly
•	Speak quietly
•	Put your phone in silent mode or at least not loud mode
•	Talk on the phone outside the open space
•	Do not smoke in the open space
•	Do not keep food that smelling on the tables." },
                }
            },
            new Chapter
            {
                Title = "Chapter 5",
                OrderId = 7,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 5.1, Title = "Work ethics", Text = @"We stick to the rules of generally accepted professional ethics at the workplace and while communicating with customers. We expect our employees to be tolerant and reasonable and to avoid abusive language or jokes. Our employees must be guided by interests of the Company while performing their working duties.
Alcohol
Drinking alcohol during working hours is prohibited, unless it is allowed in certain cases by Top Managers. All entertainment events in the Company should take place after 6 pm and only with approval from Top Managers.
Drugs
Employees are not allowed to take illegal or not prescribed by responsible people medicine at work. They are also not allowed to bring and store such substances at the workplace." },
                    new Article { UnitNumber = 5.2, Title = "Confidentiality", Text = @"Inside information about the Company, your salary, information about customers and their projects,  should be kept confidential.
Employees are not entitled to spread or use inside information about the Company and its customers to their own benefit or to the benefit of any third party except for the Company. Exoft receives and store personal information about employees only for personal needs and development of employees. The access to inside information and personal data is limited. Information obtained by employees in the process of performing their duties cannot be used to personal benefit or to the benefit of third parties. Any information which is not public, should be protected, even in cases not covered by the agreement. This could be information about the Company, its employees and third parties." },
                    new Article { UnitNumber = 5.3, Title = "Inventions and Intellectual property", Text = @"Any kind of intellectual property, such as works, projects and inventions done by employees during period of working with Exoft, which can be copied belong to the Company, according to generally common rules and legislation." }
                }
            },
            new Chapter
            {
                Title = "Chapter 6",
                OrderId = 8,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 6.1, Title = "Referral Program", Text = @"Our referral program provides compensation for successful candidate recommendation, according to the level of the candidate: Junior – 50$, Middle – 100$, Senior – 200$.  
Compensation will be paid after successful passing of the probationary period by the candidate." },
                    new Article { UnitNumber = 6.2, Title = "Benefits", Text = @"Entertainments: 
•	Ping-pong
•	Massage 
•	Playroom with Sony Playstation
•	Corporate social activities (parties, mafia club, Board Games)
 
Education:
•	Free English courses
•	Internal knowledge sharing events
•	50% compensation for external educational events
•	Professional and competent environment
•	Mentorship programs
•	Online library

Office:
•	Free car and bicycle parking
•	Comfortable and modern office
•	Free tea, coffee, milk, and fresh fruits/cookies
 
Compensation package:
•	Vacation (18 working days per year)
•	Paid sick leaves
•	Flexible working hours" }
                }
            },
            new Chapter
            {
                Title = "Chapter 7",
                OrderId = 9,
                Articles = new List<Article>
                {
                    new Article { UnitNumber = 7.1, Title = "Termination of employment", Text = @"Notice of dismissal from employment
During the probation term the notice period is a week time before actual termination of employment and it works for both parties. After probation period this notice period is one month, as it is stated in the Agreement.
Voluntary termination
If employee has an intention to quit the cooperation with the Company he/she has to inform HR manager in time lines mentioned above. If employee is willing to quit earlier than within the mentioned period, it could be possible if this does not affect business processes in the Company. In this case those days will not be paid for. 
Involuntary termination
If an the Company has an intention to terminate cooperation it should also notify employee within the notice period. In case of major violations employee can be fired without warning. In both situations, employee is  informed about the decision and invited for meeting to discuss this issue. If authorities decide to fire employee, they have to inform him/her the date of the last working day (which is determined by the rules mentioned above).
Delegation of responsibilities
To ensure smooth running of business processes, one should delegate his/her work duties to responsible person right after the decision to quit. Under ideal circumstances there should be a plan of transferring responsibilities." }
                }
            },
            new Chapter
            {
                Title = "Summary",
                OrderId = 10,
                Articles = new List<Article>
                {
                    new Article { Title = "", Text = @"Corporate values stated in this reference book are very important for Exoft company. Revision, changes and updates should be done when necessary. Following corporate rules by employees is also an important factor during performance reviews. Any changes made in the future should immediately be announced to all employees. 
The organization can develop only through the development of its employees." }
                }
            }
        };
    }
}
