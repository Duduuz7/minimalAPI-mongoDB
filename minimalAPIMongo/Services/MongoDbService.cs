using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDbService
    {
        /// <summary>
        /// Armazenar a configuração da aplicação
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        ///  Armazena uma referência ao MongoDb
        /// </summary>
        private readonly IMongoDatabase _database;
        
        /// <summary>
        /// Contém a configuração necessária para acesso ao mongo DB
        /// </summary>
        /// <param name="configuration">Obj com a config da aplicação</param>
        public MongoDbService(IConfiguration configuration)
        {
            //Atribui a config recebida em _configuration
            _configuration = configuration;

            //Acessa a string de conexão
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //Transforma a string obtida em MongoUrl
            var mongoUrl= MongoUrl.Create(connectionString);

            //Cria um client
            var mongoClient = new MongoClient(mongoUrl);

            //Obtém a referência ao MongoDb
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        //A expressao lambida diz que a primeira propriedade get retorna o _database
        /// <summary>
        /// Propriedade para acessar o bd => retorna os dados em _database
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
