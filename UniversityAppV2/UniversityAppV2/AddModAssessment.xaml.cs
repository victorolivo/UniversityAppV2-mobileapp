using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddModAssessment : ContentPage
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set; }
        public Assessment CurrentAssessment { get; set; }
        public Course CurrentCourse { get; set; }
        public bool Modify { get; set; }

        public AddModAssessment(bool modify, Course c, Assessment a)
        {
            InitializeComponent();
            BindingContext = this;
            CurrentAssessment = a;
            CurrentCourse = c;
            Modify = modify;
            DueDateField.MinimumDate = CurrentCourse.StartDate;
            DueDateField.MaximumDate = CurrentCourse.EndDate;
            DueDate = CurrentCourse.EndDate;

            if (modify)
                prefillFields();
            else
                PageTitleLbl.Text = "Add Assessment";
        }

        private void prefillFields()
        {
            PageTitleLbl.Text = "Modify Assessment";
            NameField.Text = CurrentAssessment.Name;
            TypeField.SelectedItem = CurrentAssessment.Type;
            DueDateField.Date = CurrentAssessment.DueDate;
        }

        private void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            if (FormValid())
            {
                if (Modify)
                {
                    CurrentAssessment.Name = Name;
                    CurrentAssessment.Type = Type;
                    CurrentAssessment.DueDate = DueDate;

                    TermsDB.UpdateAssessment(CurrentAssessment);
                }
                else
                {
                    Assessment a = new Assessment
                    {
                        Name = this.Name,
                        DueDate = this.DueDate,
                        Type = this.Type,
                        CourseId = CurrentCourse.Id
                    };

                    TermsDB.AddAssessment(a);
                }

                Navigation.PopAsync();
            }
        }

        private bool FormValid()
        {
            //Input Validations
            if (string.IsNullOrWhiteSpace(Name))
            {
                App.Current.MainPage.DisplayAlert("Invalid Assessment Name", "Please enter a name for the assessment", "Ok");
                return false;
            }
            else if (Type == null)
            {
                App.Current.MainPage.DisplayAlert("Invalid Assessment Type", "Please select a type for this assessment", "Ok");
                return false;
            }
            return true;
        }
    }
}