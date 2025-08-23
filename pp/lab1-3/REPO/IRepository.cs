using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REPO
{
    public interface IRepository : IDisposable             //  наследует  IDisposable
    {
        List<WSRef> getAllWSRef();                      // получить весь перечень Интернет-ресурсов 
        List<Comment> getAllComment();                    // получить все комментарии  
        bool addWSRef(WSRef wsRef);              // добавить новый  Интернет-ресурс  
        bool addComment(Comment comment);        // добавить комментарий   
    }

    public class WSRef
    {
        [Key]
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public int? Plus { get; set; }
        public int? Minus { get; set; }
        public List<Comment>? Comments { get; set; } // для FK
    }

    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Stamp { get; set; }
        public string? Commtext { get; set; }

        [ForeignKey("WSRef")] //  для FK ->  WSRef(PK) 
        public int WSrefId { get; set; }   //  для FK 
        public WSRef? WSref { get; set; }   //  для FK   
    }
}
