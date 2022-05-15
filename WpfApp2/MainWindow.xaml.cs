using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace WpfApp2
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();
		}
		public MySqlConnection con;
		public MySqlDataReader reader;
		public bool TryConnect()
		{

			MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();

			String loginUser = LoginTextBox.Text;
			String passUser = password.Password;

			_connectionString.AllowBatch = true;
			_connectionString.Server = "localhost";
			_connectionString.Database = "nabiullinakr";
			_connectionString.UserID = loginUser;
			_connectionString.ConnectionTimeout = 30;
			_connectionString.Password = passUser;
			con = new MySqlConnection(_connectionString.ConnectionString);
			try
			{
				con.Open();
				connect.Content = "Подключение к БД выполнено";
				expcon.IsExpanded = false;
				return true;

			}
			catch
			{
				if (con != null)
				{
					connect.Content = "Неверные данные";
					LoginTextBox.Text = "";
					password.Password = "";
					con.Close();
				}
				return false;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			TryConnect();
			try
			{
				string query = "SELECT MAX(id_taxpayer) AS id_taxpayer FROM taxpayer";
				MySqlCommand command = new MySqlCommand(query, con);
				reader = command.ExecuteReader();

				List<string[]> data = new List<string[]>();

				while (reader.Read())
				{
					data.Add(new string[1]);
					for (int i = 0; i < reader.FieldCount; i++)
					{
						data[data.Count - 1][i] = reader[i].ToString();
					}
				}
				reader.Close();

				List<string> fio = new List<string>();
				int id = Convert.ToInt32(data[0][0]);

				for (int i = 1; i <= id; i++)
				{
					query = "SELECT taxpayer.surname, taxpayer.name,taxpayer.patronymic FROM taxpayer where id_taxpayer=" + i + ";";
					command = new MySqlCommand(query, con);
					reader = command.ExecuteReader();

					data.Clear();

					while (reader.Read())
					{
						data.Add(new string[3]);
						for (int j = 0; j < reader.FieldCount; j++)
						{
							data[data.Count - 1][j] = reader[j].ToString();
						}
					}
					reader.Close();
					fio.Add(data[0][0] + " " + data[0][1] + " " + data[0][2]);

				}
				paytax.ItemsSource = fio;

				////////////////////////////////////////////

				query = "SELECT MAX(id_tax) AS id_tax FROM tax";
				command = new MySqlCommand(query, con);
				reader = command.ExecuteReader();

				data.Clear();


				while (reader.Read())
				{
					data.Add(new string[1]);
					for (int i = 0; i < reader.FieldCount; i++)
					{
						data[data.Count - 1][i] = reader[i].ToString();
					}
				}
				reader.Close();

				List<string> tax = new List<string>();
				id = Convert.ToInt32(data[0][0]);

				for (int i = 1; i <= id; i++)
				{
					query = "SELECT tax.name FROM tax where id_tax=" + i + ";";
					command = new MySqlCommand(query, con);
					reader = command.ExecuteReader();

					data.Clear();

					while (reader.Read())
					{
						data.Add(new string[1]);
						for (int j = 0; j < reader.FieldCount; j++)
						{
							data[data.Count - 1][j] = reader[j].ToString();
						}
					}
					reader.Close();
					tax.Add(data[0][0]);

				}
				paytax1.ItemsSource = tax;

				/////////////////////////////////////////////

				query = "SELECT MAX(id_privilege) AS id_privilege FROM privilege";
				command = new MySqlCommand(query, con);
				reader = command.ExecuteReader();

				data.Clear();


				while (reader.Read())
				{
					data.Add(new string[1]);
					for (int i = 0; i < reader.FieldCount; i++)
					{
						data[data.Count - 1][i] = reader[i].ToString();
					}
				}
				reader.Close();

				List<string> privilege = new List<string>();
				id = Convert.ToInt32(data[0][0]);

				for (int i = 1; i <= id; i++)
				{
					query = "SELECT privilege.name FROM privilege where id_privilege=" + i + ";";
					command = new MySqlCommand(query, con);
					reader = command.ExecuteReader();

					data.Clear();

					while (reader.Read())
					{
						data.Add(new string[1]);
						for (int j = 0; j < reader.FieldCount; j++)
						{
							data[data.Count - 1][j] = reader[j].ToString();
						}
					}
					reader.Close();
					privilege.Add(data[0][0]);

				}
				paypriv.ItemsSource = privilege;
			}
			catch
			{
				connect.Content = "Неверные данные";
			}

		}


		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				try
				{
					string query = _SQLCommand.Text;

					using (MySqlCommand command = new MySqlCommand(query, con))
					{
						DataTable dt = new DataTable();
						MySqlDataAdapter _da = new MySqlDataAdapter(command);
						_da.Fill(dt);
						maingrid.ItemsSource = dt.DefaultView;
					}
				}

				catch
				{
					connect.Content = "Проверьте SQL команду";

				}
			}
			catch
			{
				connect.Content = "Выполните вход";
			}

		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{


				string curItem = city.SelectedItem.ToString();
				string a = "'";

				string id = "select taxpayer.surname as " + a + "Фамилия" + a + ", taxpayer.name as " + a + "Имя" + a + ", taxpayer.patronymic as " + a + "Отчество" + a + ",city.city_name as " + a + "Город" + a + ",taxpayer.street as " + a + "Улица" + a + ", " +
				"taxpayer.house as " + a + "Дом" + a + "," + "taxpayer.apartment as" + a + "Квартира" + a + " from city, taxpayer " +
				"where city.idcity = taxpayer.id_city and city_name= " + a + curItem + a + ';';
				sqllabel.Content = id;
				using (MySqlCommand command2 = new MySqlCommand(id, con))
				{
					DataTable dt = new DataTable();
					MySqlDataAdapter _da1 = new MySqlDataAdapter(command2);
					_da1.Fill(dt);
					taxpayergrid.ItemsSource = dt.DefaultView;
				}
			}
			catch
			{
				connect.Content = "Выполните вход";
			}

		}


		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{


				string query = "SELECT city_name FROM nabiullinakr.city";
				MySqlCommand command = new MySqlCommand(query, con);
				reader = command.ExecuteReader();
				List<string[]> data = new List<string[]>();




				while (reader.Read())
				{
					data.Add(new string[2]);
					for (int i = 0; i < reader.FieldCount; i++)
					{
						data[data.Count - 1][i] = reader[i].ToString();
					}
				}
				string writePath = @"K:\vivod.txt";
				using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
				{
					for (int j = 0; j < reader.FieldCount; j++)
					{
						ObservableCollection<string> phones = new ObservableCollection<string> { };
						for (int i = 0; i < data.Count; i++)
						{
							sw.WriteLine(data[i][j]);
							phones.Add(data[i][j]);
						}
						city.ItemsSource = phones;
						city1.ItemsSource = phones;
					}
				}
			}
			catch
			{

				connect.Content = "Выполните вход";
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			try
			{


				var result_surname = surname.Text.Any(c => (c >= 33 && c <= 127));
				var result_name = name.Text.Any(c => (c >= 33 && c <= 127));
				var result_patronymic = patronymic.Text.Any(c => (c >= 33 && c <= 127));
				var result_street = street.Text.Any(c => (c >= 33 && c <= 127));
				var result_house = house.Text.Any(c => (c >= 32 && c <= 47) || c >= 58);
				var result_apartment = apartment.Text.Any(c => (c >= 32 && c <= 47) || c >= 58);
				string a = "'";
				if (surname.Text == "" || result_surname == true)
				{
					addtaxpayerlabel.Content = "Неверная Фамилия";
				}
				else
				{
					if (name.Text == "" || result_name == true)
					{
						addtaxpayerlabel.Content = "Неверное имя";
					}
					else
					{
						if (patronymic.Text == "" || result_patronymic == true)
						{
							addtaxpayerlabel.Content = "Неверное отчество";
						}
						else
						{
							if (city1.SelectedIndex <= -1)
							{
								addtaxpayerlabel.Content = "Не указан город";
							}
							else
							{
								if (street.Text == "" || result_street == true)
								{
									addtaxpayerlabel.Content = "Неверная улица";
								}
								else
								{
									if (house.Text == "" || result_house == true)
									{
										addtaxpayerlabel.Content = "Неверный номер дома";
									}
									else
									{
										if (apartment.Text == "" || result_apartment == true)
										{
											addtaxpayerlabel.Content = "Неверный номер квартиры";
										}
										else
										{
											string query = "SELECT city.idcity from city where city_name= " + a + city1.Text + a + ';';
											reader.Close();
											MySqlCommand command = new MySqlCommand(query, con);
											reader = command.ExecuteReader();

											List<string[]> data = new List<string[]>();

											while (reader.Read())
											{
												data.Add(new string[1]);
												for (int i = 0; i < reader.FieldCount; i++)
												{
													data[data.Count - 1][i] = reader[i].ToString();
												}
											}
											reader.Close();
											string numcity = data[0][0].ToString();

											//////////////////////////////////////////////////////
											query = "SELECT MAX(id_taxpayer) AS id_taxpayer FROM taxpayer";

											command = new MySqlCommand(query, con);
											reader = command.ExecuteReader();

											data = new List<string[]>();

											while (reader.Read())
											{
												data.Add(new string[1]);
												for (int i = 0; i < reader.FieldCount; i++)
												{
													data[data.Count - 1][i] = reader[i].ToString();
												}
											}
											reader.Close();
											string id = (data[0][0]);
											int an = Convert.ToInt32(id) + 1;

											////////////////////////////////


											query = "INSERT INTO `nabiullinakr`.`taxpayer` (`id_taxpayer`, `surname`, `name`, `patronymic`, `id_city`, `street`, `house`,`apartment`) VALUES(" + a + an + a + ',' + a + surname.Text + a + ',' + a + name.Text + a + ',' + a + patronymic.Text + a + ',' + a + numcity + a + ',' + a + street.Text + a + ',' + a + house.Text + a + "," + a + apartment.Text + a + ");";

											using (command = new MySqlCommand(query, con))
											{
												DataTable dt = new DataTable();
												MySqlDataAdapter _da = new MySqlDataAdapter(command);
												_da.Fill(dt);
												taxpayergrid.ItemsSource = dt.DefaultView;
											}
											reader.Close();
											addtaxpayerlabel.Content = "Налогоплательщик добавлен";
											query = "SELECT taxpayer.surname as 'Фамилия', taxpayer.name as 'Имя', taxpayer.patronymic as 'Отчество', city.city_name as 'Город', taxpayer.street as 'Улица', taxpayer.house as 'Дом', taxpayer.apartment as 'Квартира'   FROM nabiullinakr.taxpayer, city where city.idcity=taxpayer.id_city;";
											using (command = new MySqlCommand(query, con))
											{
												DataTable dt = new DataTable();

												MySqlDataAdapter _da = new MySqlDataAdapter(command);
												_da.Fill(dt);
												taxpayergrid.ItemsSource = dt.DefaultView;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
				connect.Content = "Выполните вход";
			}

		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			try
			{

				string a = "'";

				string query = "select taxpayer.surname as " + a + "Фамилия" + a + ", taxpayer.name as " + a + "Имя" + a + ", taxpayer.patronymic as " + a + "Отчество" + a + ",city.city_name as " + a + "Город" + a + ",taxpayer.street as " + a + "Улица" + a + ", " +
				"taxpayer.house as " + a + "Дом" + a + "," + "taxpayer.apartment as" + a + "Квартира" + a + " from city, taxpayer " +
				"where taxpayer.id_city = city.idcity;";

				MySqlCommand command = new MySqlCommand(query, con);



				using (command = new MySqlCommand(query, con))
				{
					DataTable dt = new DataTable();
					MySqlDataAdapter _da = new MySqlDataAdapter(command);
					_da.Fill(dt);
					taxpayergrid.ItemsSource = dt.DefaultView;

				}

			}
			catch
			{

				connect.Content = "Выполните вход";
			}
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			try
			{


				var result_tax = tax.Text.Any(c => (c >= 33 && c <= 127));
				var result_rate = taxrate.Text.Any(c => (c >= 32 && c <= 47) || c >= 58);
				var result_term = taxterm.Text.Any(c => (c >= 33 && c <= 127));

				int year = Convert.ToInt32(DateTime.Now.Year.ToString());
				int day = Convert.ToInt32(DateTime.Now.Day.ToString());
				int month = Convert.ToInt32(DateTime.Now.Month.ToString());



				if (tax.Text == "" || result_tax == true)
				{
					addtaxlabel.Content = "Неверное название налога";
				}
				else
				{
					if (taxrate.Text == "" || result_rate == true)
					{
						addtaxlabel.Content = "Неверная процентная ставка";
					}
					else
					{
						try
						{
							List<int> lst = new List<int>(datetax.SelectedDate.ToString().Substring(0, 10).Split('.').Select(int.Parse));
							if (lst[2] < year || (lst[2] >= year && lst[1] < month) || (lst[2] >= year && lst[1] >= month && lst[0] < day))
							{
								addtaxlabel.Content = "Неверная дата";
							}
							else
							{
								if (taxterm.Text == "" || result_term == true)
								{
									addtaxlabel.Content = "Неверный параметр взимания налога";
								}
								else
								{
									addtaxlabel.Content = "Налог добавлен";

									string query = "SELECT MAX(id_tax) AS id_tax FROM tax";

									MySqlCommand command = new MySqlCommand(query, con);
									reader = command.ExecuteReader();

									List<string[]> data = new List<string[]>();

									while (reader.Read())
									{
										data.Add(new string[1]);
										for (int i = 0; i < reader.FieldCount; i++)
										{
											data[data.Count - 1][i] = reader[i].ToString();
										}
									}
									reader.Close();
									string id = (data[0][0]);
									int an = Convert.ToInt32(id) + 1;
									string a = "'";
									query = "INSERT INTO `nabiullinakr`.`tax` (`id_tax`, `name`, `date`, `rate`, `term`) VALUES(" + a + an + a + ',' + a + tax.Text + a + ',' + a + lst[2] + "-" + lst[1] + "-" + lst[0] + a + ',' + a + taxrate.Text + a + ',' + a + taxterm.Text + a + ");";
									using (command = new MySqlCommand(query, con))
									{
										DataTable dt = new DataTable();
										MySqlDataAdapter _da = new MySqlDataAdapter(command);
										_da.Fill(dt);
										taxgrid.ItemsSource = dt.DefaultView;
									}
									reader.Close();
									query = "	SELECT tax.name as" + a + "Название налога" + a + ", tax.date as " + a + "Дата оплаты" + a + ", tax.rate as" + a + "Процентная ставка" + a + ", tax.term as " + a + "Параметр взимания" + a + " FROM nabiullinakr.tax;";
									using (command = new MySqlCommand(query, con))
									{
										DataTable dt = new DataTable();
										MySqlDataAdapter _da = new MySqlDataAdapter(command);
										_da.Fill(dt);
										taxgrid.ItemsSource = dt.DefaultView;
									}
								}
							}
						}
						catch
						{
							addtaxlabel.Content = "Неверная дата";
						}

					}
				}
			}
			catch
			{
				connect.Content = "Выполните вход";
			}
		}

		private void alltax_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				string a = "'";
				string query = "	SELECT tax.name as" + a + "Название налога" + a + ", tax.date as " + a + "Дата оплаты" + a + ", tax.rate as" + a + "Процентная ставка" + a + ", tax.term as " + a + "Параметр взимания" + a + " FROM nabiullinakr.tax;";
				//SELECT tax.name as "Название налога", tax.date as "Дата оплаты", tax.rate as "Процентная ставка", tax.term as "Параметр взимания" FROM nabiullinakr.tax;
				MySqlCommand command = new MySqlCommand(query, con);



				using (command = new MySqlCommand(query, con))
				{
					DataTable dt = new DataTable();
					MySqlDataAdapter _da = new MySqlDataAdapter(command);
					_da.Fill(dt);
					taxgrid.ItemsSource = dt.DefaultView;
				}
			}
			catch
			{

				connect.Content = "Выполните вход";
			}
		}

		private void allpriv_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				string a = "'";
				string query = "	SELECT privilege.name as" + a + "Название льготы" + a + ", privilege.rate as" + a + "Процентная ставка" + a + " FROM nabiullinakr.privilege;";

				MySqlCommand command = new MySqlCommand(query, con);



				using (command = new MySqlCommand(query, con))
				{
					DataTable dt = new DataTable();
					MySqlDataAdapter _da = new MySqlDataAdapter(command);
					_da.Fill(dt);
					privgrid.ItemsSource = dt.DefaultView;
				}
			}
			catch
			{

				connect.Content = "Выполните вход";
			}
		}

		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			try
			{


				var result_priv = priv.Text.Any(c => (c >= 33 && c <= 127));
				var result_rate = privrate.Text.Any(c => (c >= 32 && c <= 47) || c >= 58);

				if (priv.Text == "" || result_priv == true)
				{
					addprivlabel.Content = "Неверное название льготы";
				}
				else
				{
					if (privrate.Text == "" || result_rate == true)
					{
						addprivlabel.Content = "Неверная процентная ставка";
					}
					else
					{
						addtaxlabel.Content = "Налог добавлен";

						string query = "SELECT MAX(id_privilege) AS id_privilege FROM privilege";

						MySqlCommand command = new MySqlCommand(query, con);
						reader = command.ExecuteReader();

						List<string[]> data = new List<string[]>();

						while (reader.Read())
						{
							data.Add(new string[1]);
							for (int i = 0; i < reader.FieldCount; i++)
							{
								data[data.Count - 1][i] = reader[i].ToString();
							}
						}
						reader.Close();
						string id = (data[0][0]);
						int an = Convert.ToInt32(id) + 1;
						string a = "'";
						query = "INSERT INTO `nabiullinakr`.`privilege` (`id_privilege`, `name`,  `rate`) VALUES(" + a + an + a + ',' + a + priv.Text + a + ',' + a + privrate.Text + a + ");";
						using (command = new MySqlCommand(query, con))
						{
							DataTable dt = new DataTable();
							MySqlDataAdapter _da = new MySqlDataAdapter(command);
							_da.Fill(dt);
							privgrid.ItemsSource = dt.DefaultView;
						}
						reader.Close();
						addprivlabel.Content = "Добавлена льгота";

						query = "	SELECT privilege.name as" + a + "Название льготы" + a + ", privilege.rate as" + a + "Процентная ставка" + a + " FROM nabiullinakr.privilege;";
						using (command = new MySqlCommand(query, con))
						{
							DataTable dt = new DataTable();
							MySqlDataAdapter _da = new MySqlDataAdapter(command);
							_da.Fill(dt);
							privgrid.ItemsSource = dt.DefaultView;
						}
					}
				}
			}
			catch
			{
				connect.Content = "Выполните вход";
			}
		}

		private void allpay_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				string a = "'";

				string query = "SELECT payment_details.amount as 'Сумма платежа',payment_details.date as 'Дата оплаты',taxpayer.surname as 'Фамилия',taxpayer.name as 'Имя',taxpayer.patronymic as 'Отчество',tax.name as 'Название налога',privilege.name as 'Льгота' FROM nabiullinakr.payment_details, tax,taxpayer, privilege where payment_details.id_taxpayer_details=taxpayer.id_taxpayer and payment_details.id_tax_details=tax.id_tax and payment_details.id_privilege_details=privilege.id_privilege;";


				MySqlCommand command = new MySqlCommand(query, con);


				using (command = new MySqlCommand(query, con))
				{
					DataTable dt = new DataTable();
					MySqlDataAdapter _da = new MySqlDataAdapter(command);
					_da.Fill(dt);
					paygrid.ItemsSource = dt.DefaultView;
				}
			}
			catch
			{

				connect.Content = "Выполните вход";
			}
		}

		private void Paytax_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void Button_Click_7(object sender, RoutedEventArgs e)
		{
			string query = "SELECT MAX(id_payment_details) AS id_payment_details FROM payment_details";
			MySqlCommand command = new MySqlCommand(query, con);
			reader = command.ExecuteReader();

			List<string[]> data = new List<string[]>();


			while (reader.Read())
			{
				data.Add(new string[1]);
				for (int i = 0; i < reader.FieldCount; i++)
				{
					data[data.Count - 1][i] = reader[i].ToString();
				}
			}
			reader.Close();

			List<string> privilege = new List<string>();
			int id = Convert.ToInt32(data[0][0]) + 1;

			var result_amount = amount.Text.Any(c => (c >= 32 && c <= 47) || c >= 58);
			int fio = Convert.ToInt32(paytax.SelectedIndex.ToString()) + 1;
			int tax = Convert.ToInt32(paytax1.SelectedIndex.ToString()) + 1;
			int priv = Convert.ToInt32(paypriv.SelectedIndex.ToString()) + 1;

			int year = Convert.ToInt32(DateTime.Now.Year.ToString());
			int day = Convert.ToInt32(DateTime.Now.Day.ToString());
			int month = Convert.ToInt32(DateTime.Now.Month.ToString());



			if (amount.Text == "" || Convert.ToInt32(amount.Text) < 0 || result_amount == true)
			{
				addpay.Content = "Неверная сумма платежа";
			}
			else
			{
				if (paytax.SelectedIndex <= -1)
				{
					addpay.Content = "Выберите налогоплательщика";
				}
				else
				{
					if (paytax1.SelectedIndex <= -1)
					{
						addpay.Content = "Выберите налог";
					}
					else
					{
						try
						{
							List<int> lst = new List<int>(datepay.SelectedDate.ToString().Substring(0, 10).Split('.').Select(int.Parse));
							if (lst[2] < year || (lst[2] >= year && lst[1] < month) || (lst[2] >= year && lst[1] >= month && lst[0] < day))
							{
								addpay.Content = "Неверная дата";
							}
							else
							{
								if (paypriv.SelectedIndex <= -1)
								{

									query = " INSERT INTO `nabiullinakr`.`payment_details` (`id_payment_details`," + "\n" +
									"`amount`, `date`, `id_taxpayer_details`, `id_tax_details`,`id_privilege_details` )" + "\n" +
									"VALUES('" + id + "', '" + amount.Text + "', '" + lst[2] + "-" + lst[1] + "-" + lst[0] + "\n" +
									"', '" + fio + "', '" + tax + "',' 0 '); ";

									command = new MySqlCommand(query, con);



									using (command = new MySqlCommand(query, con))
									{
										DataTable dt = new DataTable();
										MySqlDataAdapter _da = new MySqlDataAdapter(command);
										_da.Fill(dt);
										paygrid.ItemsSource = dt.DefaultView;

									}


									query = "SELECT payment_details.amount as 'Сумма платежа',payment_details.date as 'Дата оплаты',taxpayer.surname as 'Фамилия',taxpayer.name as 'Имя',taxpayer.patronymic as 'Отчество',tax.name as 'Название налога',privilege.name as 'Льгота' FROM nabiullinakr.payment_details, tax,taxpayer, privilege where payment_details.id_taxpayer_details=taxpayer.id_taxpayer and payment_details.id_tax_details=tax.id_tax and payment_details.id_privilege_details=privilege.id_privilege;";

									command = new MySqlCommand(query, con);

									using (command = new MySqlCommand(query, con))
									{
										DataTable dt = new DataTable();
										MySqlDataAdapter _da = new MySqlDataAdapter(command);
										_da.Fill(dt);
										paygrid.ItemsSource = dt.DefaultView;
									}
								}
								else
								{
									query = " INSERT INTO `nabiullinakr`.`payment_details` (`id_payment_details`," + "\n" +
									"`amount`, `date`, `id_taxpayer_details`, `id_tax_details`,`id_privilege_details` )" + "\n" +
									"VALUES('" + id + "', '" + amount.Text + "', '" + lst[2] + "-" + lst[1] + "-" + lst[0] + "\n" +
									"', '" + fio + "', '" + tax + "','" + priv + "'); ";

									command = new MySqlCommand(query, con);



									using (command = new MySqlCommand(query, con))
									{
										DataTable dt = new DataTable();
										MySqlDataAdapter _da = new MySqlDataAdapter(command);
										_da.Fill(dt);
										paygrid.ItemsSource = dt.DefaultView;

									}


									query = "SELECT payment_details.amount as 'Сумма платежа',payment_details.date as 'Дата оплаты',taxpayer.surname as 'Фамилия',taxpayer.name as 'Имя',taxpayer.patronymic as 'Отчество',tax.name as 'Название налога',privilege.name as 'Льгота' FROM nabiullinakr.payment_details, tax,taxpayer, privilege where payment_details.id_taxpayer_details=taxpayer.id_taxpayer and payment_details.id_tax_details=tax.id_tax and payment_details.id_privilege_details=privilege.id_privilege;";

									command = new MySqlCommand(query, con);

									using (command = new MySqlCommand(query, con))
									{
										DataTable dt = new DataTable();
										MySqlDataAdapter _da = new MySqlDataAdapter(command);
										_da.Fill(dt);
										paygrid.ItemsSource = dt.DefaultView;
									}

								}
							}
							addpay.Content = "Данные платежа добавлены";
						}
						catch
						{
							addpay.Content = "Выберите дату";
						}

					}

				}
			}
		}
	}
}
//сохранение изменений
//dbAdapter1.Update(dataTable);

