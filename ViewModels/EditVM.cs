using System.Collections.ObjectModel;

namespace ColorMorph.ViewModels
{
    public class Category
    {
        public string Name { get; set; }
    }
    public class EditVM : BaseVM
    {
        public ObservableCollection<Category> Categories { get; private set; }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public EditVM(Stream imageStream)
        {
            ImageSource = ImageSource.FromStream(() => imageStream);
            CreateCategories();
        }

        private void CreateCategories()
        {
            Categories = new ObservableCollection<Category>
            {
                new Category { Name = "Detectare Fețe" },
                new Category { Name = "Procesare Imagini" },
                new Category { Name = "Detecție Margini" },
                new Category { Name = "Segmentare Contururi" },
                new Category { Name = "Detectare Mișcare" },
                new Category { Name = "Detectare Obiecte" },
                new Category { Name = "Recunoaștere Text (OCR)" },
                new Category { Name = "Filtrare Caracteristici" },
                new Category { Name = "Transformări Morfologice" },
                new Category { Name = "Reconstrucție 3D" },
                new Category { Name = "Transformări Fourier" },
                new Category { Name = "Template Matching" },
                new Category { Name = "Calibration" }
            };
        }
    }
}