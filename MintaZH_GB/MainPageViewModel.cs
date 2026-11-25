using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MintaZH_GB
{
    [QueryProperty(nameof(EditedStudent), "editedStudent")]
    public partial class MainPageViewModel:ObservableObject
    {
        public ObservableCollection<Student> ActiveStudents { get; set; }
        public ObservableCollection<Student> PassiveStudents { get; set; }

        [ObservableProperty]
        Student selectedStudent;

       public Student EditedStudent
        {
            set
            {
                if (value != null)
                {
                    if (SelectedStudent != null)
                    {
                        DeleteSelected();
                    }
                    if (value.IsActive)
                    {
                        ActiveStudents.Add(value);
                    }else
                    {
                        PassiveStudents.Add(value);
                    }
                    OnPropertyChanged(nameof(AllStudentsAverage));
                }
            }
        }

        public double AllStudentsAverage
        {
            get
            {
                if (ActiveStudents.Count + PassiveStudents.Count > 0)
                {
                    return ActiveStudents.Concat(PassiveStudents).Average(s => s.Average);
                }
                else return 0;
                
            }
        }

        public MainPageViewModel()
        {
            ActiveStudents = new ObservableCollection<Student>();
            PassiveStudents = new ObservableCollection<Student>();
        }

        [RelayCommand]
        void DeleteSelected()
        {
            if (SelectedStudent != null)
            {
                ActiveStudents.Remove(SelectedStudent);
                PassiveStudents.Remove(SelectedStudent);
                SelectedStudent = null;
                OnPropertyChanged(nameof(AllStudentsAverage));
            }
        }

        [RelayCommand]
        async Task NewStudentAsync()
        {
            SelectedStudent = null;
            await Shell.Current.GoToAsync("edit", new ShellNavigationQueryParameters() { { "student", new Student() } });

        }

        [RelayCommand]
        async Task EditStudentAsync()
        {
            await Shell.Current.GoToAsync("edit", new ShellNavigationQueryParameters() { { "student", SelectedStudent.GetCopy() } });

        }

        [RelayCommand]
        async Task ShareAverageAsync()
        {
            await Share.Default.RequestAsync(new ShareTextRequest()
            {
                Text = "Average grade of all students: " + AllStudentsAverage
            });
        }

        [RelayCommand]
        async Task ShareListAsync()
        {
            SaveToJson(ActiveStudents, "activeStudents.json");
            await Share.Default.RequestAsync(new ShareFileRequest()
            {
                File = new ShareFile(Path.Combine(FileSystem.Current.AppDataDirectory, "activeStudents.json"))
            });
        }


        public void SaveToJson(IEnumerable<Student> students, string filename)
        {
            File.WriteAllText(
                Path.Combine(FileSystem.Current.AppDataDirectory, filename),
                JsonSerializer.Serialize(students));
            
        }

        public void SaveToJson(string filename)
        {
            SaveToJson(ActiveStudents.Concat(PassiveStudents), filename);

        }

        public void LoadFromJson(string filename)
        {
            string path = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                List<Student> sList = JsonSerializer.Deserialize<List<Student>>(json);
                if (sList != null && sList.Count > 0)
                {
                    foreach (var item in sList)
                    {
                        if (item.IsActive) ActiveStudents.Add(item);
                        else PassiveStudents.Add(item);
                    }
                    OnPropertyChanged(nameof(AllStudentsAverage));
                }
                
            }
        }
    }
}
