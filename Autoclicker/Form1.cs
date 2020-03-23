using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoclicker
{
    // Класс нарушает SRP и делает слишком много!
    // Импорт ДЛЛ следует вынести в отдельный класс
    // Вынести генерацию интерфейса
    // Венести обработку каждого действия мыши
    // Класс Form1 должен выступать в качестве стартовой точки
 
    public partial class Form1 : Form
    {
        int loc_y, loc_x;
        bool isGrab;
        Point margin;
        Timer t;
        Button button;

        List<Control> elementControl;
        Dictionary<string, List<Control>> groupCOntrols;

        //[TEST]
        public static List<EventHandler> eventClick; // На каждом клике висит событие

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var parms = base.CreateParams;
        //        parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
        //        return parms;
        //    }
        //}

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int nIndex);

        [Flags]
        public enum MouseFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        public Form1()
        {
            InitializeComponent();

            isGrab = false;
            margin = new Point();
            loc_x = 12;
            loc_y = 23;
            margin.X = 12;
            margin.Y = 40;

            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);
           
            elementControl = new List<Control>();
            groupCOntrols = new Dictionary<string, List<Control>>();

            eventClick = new List<EventHandler>();
            new Class1(); // initialize

            t = new Timer();
            t.Interval = 50;
            t.Tick += new EventHandler(t_Tick);
	    t.Start();
	    t.Enabled = false;	

            this.AutoScroll = true;
		
		// Класс должен выполняться в отдельном потоке
            MouseHook.LocalHook = false;
            MouseHook.InstallHook();
            MouseHook.MouseDown += MouseHook_MouseDown;
        }

        int index = 0;

        private void Start_Click(object sender, EventArgs e)
        {
           
	    // Класс должен выполняться в отдельном потоке
      
            for(int i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                foreach (KeyValuePair<string, List<Control>> item in groupCOntrols)
                {
                    long nx = Convert.ToUInt32(item.Value[1].Text) * 65535 / GetSystemMetrics(0);
                    long ny = Convert.ToUInt32(item.Value[3].Text) * 65535 / GetSystemMetrics(1);

                    mouse_event((uint)(MouseFlags.ABSOLUTE | MouseFlags.MOVE), (uint)nx, (uint)ny, 0, UIntPtr.Zero);
                    mouse_event((uint)(MouseFlags.ABSOLUTE | MouseFlags.LEFTDOWN), (uint)nx, (uint)ny, 0, UIntPtr.Zero);

                    System.Threading.Thread.Sleep(Convert.ToInt32(item.Value[5].Text));



                    eventClick[index % 3](this, EventArgs.Empty);
                    index++;
                }
            }
            
             
        }

        private void MouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (isGrab)
            {
                t.Enabled = false;	
                isGrab = false;
                button.Text = "Grab OFF";        
                button.BackColor = Color.LightGray;
            }
               
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            MouseHook.UnInstallHook();
        }
 
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Add_Click(object sender, EventArgs e)
        {
            string guId = System.Guid.NewGuid().ToString();

            elementControl = new List<Control>();

            #region Controls

	    // Это ужасная генерация элементов управления. Нужно исправить.
            // Элементов управления может быть больше. Поэтому нужно подумать 
            // об использовании какого-нибудь порождающего паттерна.
	    
	    // Область разбить на секции при построени интерфейса. Сделать грид?
	    // Нужна разметка!

            // 0
            Label X = new Label();
            X.AutoSize = true;
            X.Location = new System.Drawing.Point(margin.X, margin.Y);
            X.Name = guId;
            X.Size = new System.Drawing.Size(14, 13);
            X.Text = "X";
            Controls.Add(X);
            elementControl.Add(X);

            // 1
            margin.X += X.Size.Width;
            TextBox coord_x = new TextBox();
            coord_x.Name = guId;
            coord_x.Location = new System.Drawing.Point(margin.X, margin.Y);           
            coord_x.Size = new System.Drawing.Size(100, 23);
            Controls.Add(coord_x);
            elementControl.Add(coord_x);

            // 2
            margin.X += coord_x.Size.Width;
            Label Y = new Label();
            Y.AutoSize = true;
            Y.Location = new System.Drawing.Point(margin.X, margin.Y);
            Y.Name = guId;
            Y.Size = new System.Drawing.Size(14, 13);
            Y.Text = "Y";
            Controls.Add(Y);
            elementControl.Add(Y);

            // 3
            margin.X += Y.Size.Width;
            TextBox coord_y = new TextBox();
            coord_y.Name = guId;
            coord_y.Location = new System.Drawing.Point(margin.X, margin.Y);
            coord_y.Size = new System.Drawing.Size(100, 23);
            Controls.Add(coord_y);
            elementControl.Add(coord_y);

            // 4
            margin.X += coord_y.Size.Width;
            Label Delay = new Label();
            Delay.AutoSize = true;
            Delay.Location = new System.Drawing.Point(margin.X, margin.Y);
            Delay.Name = guId;
            Delay.Size = new System.Drawing.Size(14, 13);
            Delay.Text = "Delay";
            Controls.Add(Delay);
            elementControl.Add(Delay);

            // 5
            margin.X += Delay.Size.Width;
            TextBox DelayValue = new TextBox();
            DelayValue.Name = guId;
            DelayValue.Location = new System.Drawing.Point(margin.X, margin.Y);
            DelayValue.Size = new System.Drawing.Size(100, 23);
            Controls.Add(DelayValue);
            elementControl.Add(DelayValue);

            // 6
            margin.X += DelayValue.Size.Width;
            Button click = new Button();
            click.Location = new System.Drawing.Point(margin.X, margin.Y);
            click.Name = guId;
            click.Size = new System.Drawing.Size(75, 23);
            click.Text = "Grab OFF";
            click.UseVisualStyleBackColor = true;
            click.Click += Grab_Click;
            Controls.Add(click);
            elementControl.Add(click);

            // 7
            margin.X += click.Size.Width;
            Button remove = new Button();
            remove.Name = guId;
            remove.Location = new System.Drawing.Point(margin.X, margin.Y);
            remove.Size = new System.Drawing.Size(28, 23);
            remove.Text = "X";
            remove.UseVisualStyleBackColor = true;
            remove.Click += Remove_Click;     
            Controls.Add(remove);
            elementControl.Add(remove);

            // 8
            //margin.X += remove.Size.Width;
            //Label state = new Label();
            //state.AutoSize = false;
            //state.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //state.Location = new System.Drawing.Point(margin.X, margin.Y);
            //state.Name = guId;
            //state.Size = new System.Drawing.Size(20, 23);
            //state.BackColor = Color.LightGray;
            //Controls.Add(state);
            //elementControl.Add(state);

            #endregion

            groupCOntrols.Add(guId, elementControl);
            
            margin.X = loc_x;
            margin.Y += loc_y;
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (!isGrab)
            {
                if (sender == null && sender.GetType() != typeof(Button))
                    return;
                
                Control deleteControl = sender as Control;

                for (int index = groupCOntrols.Count - 1; index >= 0; index--)
                {
                    var item = groupCOntrols.ElementAt(index);
                    var itemValue = item.Value;

                    if (string.Equals(deleteControl.Name, itemValue[0].Name, StringComparison.CurrentCultureIgnoreCase))
                        break;

                    foreach (var element in itemValue)
                    {                        
                        element.Location = new Point(element.Location.X, element.Location.Y - loc_y);                      
                    }
                }

                var toDelete = this.groupCOntrols[deleteControl.Name]; 
                groupCOntrols.Remove(deleteControl.Name);
                foreach (var el in toDelete)
                {
                   Controls.Remove(el);
                    el.Dispose();
                }
                

                margin.Y -= loc_y;
            }
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (elementControl == null)
                return;

            elementControl[1].Text = Cursor.Position.X.ToString();   // TextBox X
            elementControl[3].Text = Cursor.Position.Y.ToString();   // TextBox Y                
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            if (groupCOntrols.Count == 0)
                return;

            foreach (KeyValuePair<string, List<Control>> item in groupCOntrols)
            {
                foreach(Control element in item.Value)
                {
                   Controls.Remove(element);
                   element.Dispose();
                }
                margin.Y -= loc_y;
            }

            groupCOntrols.Clear();
            elementControl = new List<Control>();
        }

        private void Grab_Click(object sender, EventArgs e)
        {
            //if (sender == null && sender.GetType() != typeof(Button))
              //  return;
	    
	    if(sender == null && !(sender is Button))
 		return;

            if (isGrab == false)
            {
                button = (sender as Button);
                button.Text = "Grab ON";
                button.BackColor = Color.LawnGreen;
                isGrab = true;

                elementControl = new List<Control>();
                groupCOntrols.TryGetValue(button.Name, out elementControl);

                t.Enabled = true;	     
            }  
        }
    }
    
}
