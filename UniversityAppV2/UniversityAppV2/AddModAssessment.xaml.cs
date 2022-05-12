using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddModAssessment : ContentPage
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Assessment CurrentAssessment { get; set; }
        public Course CurrentCourse { get; set; }

        public AddModAssessment(bool modify, Course c, Assessment a)
        {
            InitializeComponent();
            BindingContext = this;
            CurrentAssessment = a;
            CurrentCourse = c;

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
        }

        private void SubmitBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}