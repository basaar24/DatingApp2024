namespace API.Data;

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using API.DataEntities;
using API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like) => context.Likes.Add(like);
    public void RemoveLike(UserLike like) => context.Likes.Remove(like);

    public async Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUSerId)
        => await context.Likes
            .Where(l => l.SourceUserId == currentUSerId)
            .Select(l => l.TargetUserId)
            .ToListAsync();

    public async Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targerUserId)
        => await context.Likes.FindAsync(sourceUserId, targerUserId);

    public async Task<IEnumerable<MemberResponse>> GetUserLikesAsync(string predicate, int userId)
    {
        var likes = context.Likes.AsQueryable();

        switch (predicate.ToLower(CultureInfo.InvariantCulture))
        {
            case "liked":
                return await likes
                    .Where(l => l.SourceUserId == userId)
                    .Select(l => l.TargetUser)
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                    .ToListAsync();
            case "likedby":
                return await likes
                    .Where(l => l.TargetUserId == userId)
                    .Select(l => l.SourceUser)
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                    .ToListAsync();
            default:
                var likeIds = await GetCurrentUserLikeIdsAsync(userId);

                return await likes
                    .Where(l => l.TargetUserId == userId && likeIds.Contains(l.SourceUserId))
                    .Select(l => l.SourceUser)
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                    .ToListAsync();
        }
    }

    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
}