using SQLite;
using PCLExt.FileStorage.Folders;

namespace bobesponja2._0.Services
{
    public class DataBaseService
    {

        public SQLiteConnection GetConexao()
        {

            var folder =
                new LocalRootFolder();

         
            var file =
                folder.CreateFile("bobburg",
                    PCLExt.FileStorage.
                        CreationCollisionOption.
                            OpenIfExists);

            return
                new SQLiteConnection(file.Path);
        }
    }
}

