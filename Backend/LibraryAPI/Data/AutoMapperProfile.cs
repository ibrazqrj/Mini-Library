using AutoMapper;
using LibraryAPI.Models;

namespace LibraryAPI.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookForCreate, BookEntity>();
            CreateMap<BookForEdit, BookEntity>();

            CreateMap<UserForRegister, UserEntity>();
            CreateMap<UserForLogin, UserEntity>();
            CreateMap<UserForDataUpdate, UserEntity>();

            CreateMap<BookForBorrow, BorrowingsEntity>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<BookForReturn, BorrowingsEntity>();

            CreateMap<Book, BookEntity>().ReverseMap();
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<Borrowings, BorrowingsEntity>().ReverseMap();
        }
    }
}
