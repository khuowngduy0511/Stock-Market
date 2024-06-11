using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api_examlpe.Dtos.Comment;
using web_api_examlpe.Models;

namespace web_api_examlpe.Mappers
{
 
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            id = commentModel.id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            StockId = commentModel.StockId
        };
    }   
        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId) 
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto) 
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
            };
        }

        }
}