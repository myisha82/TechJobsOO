using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using TechJobs.Data;
using TechJobs.Models;
using TechJobs.ViewModels;

namespace TechJobs.Controllers
{
    public class JobController : Controller
    {

        // Our reference to the data store
        private static JobData jobData;

        static JobController()
        {
            jobData = JobData.GetInstance();
        }

        // The detail display for a given Job at URLs like /Job?id=17
        public IActionResult Index(int id)
        {
            // TODO #1 - get the Job with the given ID and pass it into the view

            Job singlejob = jobData.Jobs.Find(s => s.ID == id);

            JobViewModel viewModel = new JobViewModel()
            {
                Name = singlejob.Name,
                Employer = singlejob.Employer.Value,
                Location = singlejob.Location.Value,
                CoreCompetency = singlejob.CoreCompetency.Value,
                PositionType = singlejob.PositionType.Value 
            };

            return View(viewModel);
        }

        public IActionResult New()
        {
            NewJobViewModel newJobViewModel = new NewJobViewModel();
            return View(newJobViewModel);
        }

        [HttpPost]
        public IActionResult New(NewJobViewModel newJobViewModel)
        {
            // TODO #6 - Validate the ViewModel and if valid, create a 
            // new Job and add it to the JobData data store. Then
            // redirect to the Job detail (Index) action/view for the new Job.
            if (ModelState.IsValid)
            { 
                Job newJob = new Job()
                {
                    Name = newJobViewModel.Name,
                    Employer = jobData.Employers.Find(newJobViewModel.EmployerID),
                    Location = jobData.Locations.Find(newJobViewModel.LocationID),
                    CoreCompetency = jobData.CoreCompetencies.Find(newJobViewModel.SkillID),
                    PositionType = jobData.PositionTypes.Find(newJobViewModel.PositionTypeID) 
                };

                jobData.Jobs.Add(newJob);

                return Redirect(string.Format("/Job?id={0}", newJob.ID));
        
            } 

            return View(newJobViewModel);
        }
    }
}
