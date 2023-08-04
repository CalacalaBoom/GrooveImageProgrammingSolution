using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MvvmBase;
using System.Windows.Controls;

namespace Solution
{
    public class MainVM : CommandBase
    {
        public CommandBase Painting { get; set; }

        public MainVM()
        {
            this.Painting = new CommandBase();
            this.Painting.DoCanExcute = new Func<object, bool>(o => true);
            this.Painting.DoExcute = new Action<object>(OnPaint);
        }

        private void OnPaint(object obj)
        {

        }

    }
}
