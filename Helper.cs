using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IslamovAIDBClientWPF
{
  

    public static class WorkingMySQLDBHelper
    {
        public static vmMainViewModel MainViewModel { set; get; }

        static WorkingMySQLDBHelper()
        {
            MainViewModel = new vmMainViewModel(); 
        }
    }


    public partial class vmMainViewModel : INotifyPropertyChanged
    {
        #region Свойства и методы подключения к БД
   
        private bool _isConnected;
        private string _user = "root";
        private string _password;

        private ObservableCollection<string> log_ = new ObservableCollection<string>();
        private MySqlConnection Connection;
        private MySqlDataAdapter _adapterStudent;

        private readonly string _SQLCommand;

        public event PropertyChangedEventHandler PropertyChanged;


        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public ObservableCollection<string> LOG
        {
            set
            {
                log_ = value;
                RaisePropertyChanged("LOG");
            }
            get
            {
                return log_;
            }
        }


        public string User
        {
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
            get { return _user; }
        }

        public string Password
        {
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
            get { return _password; }
        }

        public bool IsConnected
        {
            set
            {
                _isConnected = value;
                RaisePropertyChanged("Connection");
            }
            get { return _isConnected; }
        }

        //private void InitAdapter()
        //{
        //    DoSqlQuery();

        //}


        public bool TryConnect()
        {
            try
            {
                //создаем строку подключения
                MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
                //Объявленное в классе поле подключения
                Connection = null;

                //задаем логин и пароль подключения, или БД, сервер (localhost)
                _connectionString.AllowBatch = true;
                _connectionString.Server = "127.0.0.1";
                _connectionString.Database = "pm349_islamov_a_i";
                _connectionString.UserID = _user;
                _connectionString.ConnectionTimeout = 30;
                _connectionString.Password = Password;

                //создаем подключение. если нет доступа к БД, либо заданы неверные логин, пароль, имя БД
                //имя сервера метод Open выдаст исключение, которое обрабатывается в блоке Except
                Connection = new MySqlConnection(_connectionString.ConnectionString);
                Connection.Open();

                //выставляем флаг того, что подключение прошло успешно
                IsConnected = true;
                //Выводим сообщение в LOG
                LOG.Clear();
                LOG.Add("Подключение к БД выполнено");


                Console.WriteLine("TRY");

                return true;
            }

            catch
            {
                if (Connection != null)
                {
                    Connection.Close();
                }

                LOG.Add("Ошибка авторизации");
                IsConnected = false;
                return false;
            }
        }

        public DataTable DoSqlQuery(string SQL)
        {
            try
            {
                using (MySqlCommand cmdSel = new MySqlCommand(SQL, Connection))
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter _da = new MySqlDataAdapter(cmdSel);
                    _da.Fill(dt);

                    return dt;
                }
            }
            catch
            {
                if (TryConnect()==false);

                else LOG.Add("Ошибка в SQL запросе");
                return null;
            }

        }

        public DataTable DoSqlQuery()
        {
            return DoSqlQuery(_SQLCommand);
        }


        //СПРАВОЧНИКИ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public DataTable discipline { get { return DoSqlQuery("SELECT discipline.discipline_id as 'ID дисциплины', discipline.title_dis as 'Название дисциплины' FROM pm349_islamov_a_i.discipline;"); } }

        public DataTable group { get { return DoSqlQuery("SELECT `group`.group_id as 'ID группы', `group`.name_group as 'Название группы' FROM pm349_islamov_a_i.`group`;"); } }

        public DataTable teacher { get { return DoSqlQuery("SELECT teacher.teacher_id, teacher.name FROM pm349_islamov_a_i.teacher;"); } }

        #endregion
    }

}