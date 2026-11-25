using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintaZH_GB
{
    public partial class Student:ObservableObject
    {
        [ObservableProperty]
        string name;
        [ObservableProperty]
        double average;
        [ObservableProperty]
        int balance;
        [ObservableProperty]
        bool isActive;

        public Student GetCopy()
        {
            return (Student)this.MemberwiseClone();
        }
    }
}
