internal class Program
{
    private static void Main(string[] args)
    {
        Test(new DALMSQLXG.Repository());
        Test(new DALJSONXG.Repository("WSReffff.json"));
    }

    private static void Test(GREPO.IRepository repo)
    {
        using (repo)
        {
            repo.getAllWSRef().ForEach(wsRef =>
            {
                Console.WriteLine($"WSRefs: {wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });

            repo.getAllComment().ForEach(comment =>
            {
                Console.WriteLine($"Comments: {comment.Commtext}, {comment.Stamp}, {comment.WSrefId}");
            });

            var addWsResult = repo.addWSRef(new GREPO.WSRef()
            {
                Url = "https://www.belstu.by",
                Description = "БГТУ",
                Minus = 0,
                Plus = 0
            });
            Console.WriteLine(addWsResult ? "WSRefs: Add" : "WSRefs: Error Add");

            var addCommentResult = repo.addComment(new GREPO.Comment()
            {
                WSrefId = 3,
                Commtext = "test",
                Stamp = DateTime.Now
            });
            Console.WriteLine(addCommentResult ? "Comments: Add" : "Comments: Error Add");

            Console.WriteLine("After addWSRef, addComment");

            repo.getAllWSRef().ForEach(wsRef =>
            {
                Console.WriteLine($"WSRefs: {wsRef.Id}: {wsRef.Url}, {wsRef.Description}, {wsRef.Minus}, {wsRef.Plus}");
            });

            repo.getAllComment().ForEach(comment =>
            {
                Console.WriteLine($"Comments: {comment.Commtext}, {comment.Stamp}, {comment.WSrefId}");
            });

            Console.WriteLine("Finish\n\n");
            repo.Dispose();
        }
    }
}