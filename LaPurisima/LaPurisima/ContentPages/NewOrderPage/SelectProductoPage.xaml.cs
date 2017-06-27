using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaPurisima
{
	public partial class SelectProductoPage : BasePage
	{
		ObservableCollection<ItemLista> _collection;

		public SelectProductoPage()
		{
			InitializeComponent();

			_collection = new ObservableCollection<ItemLista>();

			InitViews();
		}

		async void InitViews()
		{
			_searchBar.TextChanged += (sender, e) =>
			{
				GetProductos(e.NewTextValue);
			};
			_listView.ItemSelected += (sender, e) =>
			{
				_listView.SelectedItem = null;
			};

			_listView.Refreshing += (sender, e) =>
			{
				_listView.IsRefreshing = true;
				GetProductosWeb();
			};

			GetProductos();
			await UpdateHelper.UpdateInfo();
			GetProductos();
			UpdateView();
		}

		void ContinueClicked(object sender, System.EventArgs e)
		{
			if (NextPage != null)
				NextPage(this);
		}

		void UpdateView()
		{
			if (HelperOrdenPage.Pedido != null && _items != null)
				foreach (var item in HelperOrdenPage.Pedido.productos)
					foreach (var n in _items.Where(x => x.id == item.id))
						n.cantidad = item.cantidad;

			UpdateOrder();
		}

		void UpdateOrder()
		{
			if (HelperOrdenPage.Pedido == null && PropertiesManager.GetUserInfo() != null)
				HelperOrdenPage.Pedido = new Pedido()
				{
					api_token = PropertiesManager.GetUserInfo().api_token,
					cliente_id = (int)PropertiesManager.GetUserInfo().id,
					productos = new List<Producto>()
					{

					},
				};
			else
				HelperOrdenPage.Pedido.productos = new List<Producto>()
				{

				};

			foreach (var item in _items)
			{
				HelperOrdenPage.Pedido.productos.Add(new Producto()
				{
					id = item.id,
					cantidad = item.cantidad,
					precio = item.precio,
					nombre = item.nombre,
					descripcion = item.descripcion,
					imagen = item.imagen,
				});
			}

			var total = _items.Sum(x => x.cantidad * x.precio);

			if (total == 0)
			{
				footerTotal.IsVisible = false;
			}
			else {
				footerTotal.IsVisible = true;
			}

			AnimateText(_totalLabel, string.Format("${0}", total));
		}

		void AddCantidad(object sender, System.EventArgs e)
		{
			var button = (Button)sender;
			ItemLista item = (ItemLista)button.BindingContext;
			int n = 0;
			string total = "";
			foreach (var p in _collection.Where(x => x.id == item.id))
			{
				p.cantidad++;
				n = p.cantidad;
				total = "$" + p.Total + "";
			}


			var stackEntry = (StackLayout)(button.Parent);
			var label = (Label)stackEntry.Children[1];
			label.Text = string.Format("{0}", n);
			//var gridLabel = (Grid)(button.Parent.Parent.Parent);
			//var label = (Label)gridLabel.Children[3];
			//label.Text = total;
			UpdateOrder();
		}

		void SubstractCantidad(object sender, System.EventArgs e)
		{
			var button = (Button)sender;
			ItemLista item = (ItemLista)button.BindingContext;
			int n = 0;
			string total = "";
			foreach (var p in _collection.Where(x => x.id == item.id))
			{
				p.cantidad--;
				if (p.cantidad < 0)
					p.cantidad = 0;
				n = p.cantidad;
				total = "$" + p.Total + "";
			}


			var stackEntry = (StackLayout)(button.Parent);
			var label = (Label)stackEntry.Children[1];
			label.Text = string.Format("{0}", n);
			UpdateOrder();
		}

		async void GetProductosWeb()
		{

			_productos = await ClientLaPurisima.GetObject<List<Producto>>(WEB_METHODS.GetProductos);
			if (_productos == null)
				return;

			_items.Clear();

			foreach (var item in _productos)
			{
				_items.Add(new ItemLista()
				{
					id = item.id,
					contenido = item.contenido,
					created_at = item.created_at,
					deleted_at = item.deleted_at,
					descripcion = item.descripcion,
					imagen = item.imagen,
					nombre = item.nombre,
					precio = item.precio,
					stock = item.stock,
					updated_at = item.updated_at,
				});
			}

			_collection = new ObservableCollection<ItemLista>(_items);

			_listView.ItemsSource = _collection;
			ShowNoResults(_collection.Count == 0);

			_listView.IsRefreshing = false;

			UpdateView();
		}

		async void ShowNoResults(bool v)
		{
			if (v)
			{
				await _labelNoResutls.FadeTo(1, 250, Easing.SinIn);

			}
			else {
				await _labelNoResutls.FadeTo(0, 200, Easing.SpringOut);
			}
		}

		List<Producto> _productos;
		List<ItemLista> _items;

		async void GetProductos(string where = null)
		{
			if (_productos == null || _productos.Count ==0)
			{
				_productos = RealmHelper.GetProductos();
				_items = new List<ItemLista>();

				foreach (var item in _productos)
				{
					_items.Add(new ItemLista()
					{
						id = item.id,
						contenido = item.contenido,
						created_at = item.created_at,
						deleted_at = item.deleted_at,
						descripcion = item.descripcion,
						imagen = item.imagen,
						nombre = item.nombre,
						precio = item.precio,
						stock = item.stock,
						updated_at = item.updated_at,
					});
				}

			}

			if (where != null)
			{
				var temp = _items.Where(x => x.ToString().Contains(where));
				_collection = new ObservableCollection<ItemLista>(temp);
				_listView.ItemsSource = _collection;
			}
			else {
				_collection = new ObservableCollection<ItemLista>(_items);
				_listView.ItemsSource = _collection;
			}

			ShowNoResults(_collection.Count == 0);

			if (Device.OS == TargetPlatform.Android)
				UpdateView();
		}

		async void AnimateText(Label label, string newText)
		{
			var color = label.TextColor;
			await Task.WhenAll(
				label.ColorTo(Color.Gray, Color.Black, c => label.TextColor = c, 200),

				label.ColorTo(Color.Black, Color.Gray, c => label.TextColor = c, 200));

			label.Text = newText;
		}

		class ItemLista
		{
			public int id { get; set; }
			public string nombre { get; set; }
			public string descripcion { get; set; }
			public int stock { get; set; }
			public int contenido { get; set; }
			public double precio { get; set; }
			public string imagen { get; set; }
			public string created_at { get; set; }
			public string updated_at { get; set; }
			public string deleted_at { get; set; }

			public override string ToString()
			{
				return string.Format("[Producto: id={0}, nombre={1}, descripcion={2}, stock={3}, contenido={4}, precio={5}, imagen={6}, created_at={7}, updated_at={8}, deleted_at={9}, producto_id={10}, cantidad={11}]", id, nombre, descripcion, stock, contenido, precio, imagen, created_at, updated_at, deleted_at, producto_id, cantidad);
			}

			public string Total
			{
				get
				{
					return "" + cantidad * precio;
				}

				set
				{

				}
			}

			#region PEDIDO

			public int producto_id
			{
				get
				{
					return id;
				}
			}

			public int cantidad { get; set; }
			#endregion
		}
	}
}