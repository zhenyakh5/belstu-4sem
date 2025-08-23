using REPO;

namespace DAL
{
    public class Repository : IRepository
    {
        Context context;

        public Repository()
        {
            context = new Context();
        }

        public static IRepository Create() { return new Repository(); }

        public List<Comment> getAllComment()
        {
            return context.Comments.ToList<Comment>();
        }

        public List<WSRef> getAllWSRef()
        {
            return context.WSRefs.ToList<WSRef>();
        }

        public bool addWSRef(WSRef wsRef)
        {
            bool rws = false;
            if (context.WSRefs != null)
            {
                context.Add(wsRef);
                rws = (context.SaveChanges() > 0);
            }
            return rws;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public bool addComment(Comment comment)
        {
            bool rc = false;
            if (context.Comments != null)
            {
                context.Add(comment);
                rc = (context.SaveChanges() > 0);
            }
            return rc;
        }
    }
}
