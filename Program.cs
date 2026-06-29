using LibraryApp.Data;
using LibraryApp.Dtos;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Repositories;
using LibraryApp.Services;

// ===== Day 1 Exercise: Library Member Management =====

List<Member> members = new List<Member>
{
    new Member(1, "Jane Smith", true, MemberType.Premium, 2.50m),
    new Member(2, "Bob Jones", false, MemberType.Standard, 5.00m),
    new Member(3, "Sarah Lee", true, MemberType.Standard, 0.00m),
    new Member(4, "Tom Hall", true, MemberType.Premium, 1.75m)
};

PrintReport(members);

decimal totalFees = CalculateTotalFees(members);
int activeCount = members.Count(m => m.IsActive);

Console.WriteLine("===== Report Summary =====");
Console.WriteLine($"Total Members: {members.Count}");
Console.WriteLine($"Active Members: {activeCount}");
Console.WriteLine($"Total Late Fees: ${totalFees:F2}");
Console.WriteLine();

void PrintReport(List<Member> memberList)
{
    Console.WriteLine("===== Library Member Report =====");

    foreach (Member member in memberList)
    {
        string status;
        if (member.IsActive)
            status = "Active";
        else
            status = "Inactive";

        string membershipTier;
        switch (member.MemberType)
        {
            case MemberType.Premium:
                membershipTier = "Premium";
                break;
            case MemberType.Standard:
                membershipTier = "Standard";
                break;
            default:
                membershipTier = "Unknown";
                break;
        }

        Console.WriteLine($"Member ID: {member.MemberId}");
        Console.WriteLine($"Name: {member.FullName}");
        Console.WriteLine($"Status: {status}");
        Console.WriteLine($"Membership Tier: {membershipTier}");
        Console.WriteLine($"Late Fee: ${member.LateFee:F2}");
    }
}

decimal CalculateTotalFees(List<Member> memberList)
{
    decimal total = 0;
    foreach (Member member in memberList)
    {
        total += member.LateFee;
    }
    return total;
}

// ===== Fin Day 1 Exercise =====


var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"===== Day 4 Task 1: Environment = {builder.Environment.EnvironmentName} =====");
Console.WriteLine($"Connection String in use: {builder.Configuration.GetConnectionString("LibraryConnection")}");

// Add services to the container.
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<LibraryService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// ===== Day 3 Task 7: Verify relationships with test data =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

    if (!context.Authors.Any(a => a.FullName == "George Orwell"))
    {
        // ----- Author 1: George Orwell -----
        var orwell = new Author { FullName = "George Orwell", Nationality = "British" };

        var book1 = new Book { Title = "1984", PublicationYear = 1949, PageCount = 328, Author = orwell };
        var book2 = new Book { Title = "Animal Farm", PublicationYear = 1945, PageCount = 112, Author = orwell };

        var genreFiction = new Genre { Name = "Fiction" };
        var genrePolitical = new Genre { Name = "Political Satire" };
        book1.Genres.Add(genreFiction);
        book1.Genres.Add(genrePolitical);

        // ----- Author 2: Jane Austen -----
        var austen = new Author { FullName = "Jane Austen", Nationality = "British" };

        var book3 = new Book { Title = "Pride and Prejudice", PublicationYear = 1813, PageCount = 432, Author = austen };
        var book4 = new Book { Title = "Sense and Sensibility", PublicationYear = 1811, PageCount = 409, Author = austen };
        var book5 = new Book { Title = "Emma", PublicationYear = 1815, PageCount = 474, Author = austen };

        var genreRomance = new Genre { Name = "Romance" };
        book3.Genres.Add(genreFiction);
        book3.Genres.Add(genreRomance);
        book4.Genres.Add(genreRomance);

        // ----- Members -----
        var alice = new Member { FullName = "Alice Johnson", Email = "alice.johnson@library.com", IsActive = true, MemberType = MemberType.Standard, LateFee = 0.00m };
        var carlos = new Member { FullName = "Carlos Mendez", Email = "carlos.mendez@library.com", IsActive = true, MemberType = MemberType.Premium, LateFee = 3.50m };

        // ----- Loans -----
        var loan1 = new Loan { Book = book1, Member = alice, LoanDate = DateTime.Now.AddDays(-5) };
        var loan2 = new Loan { Book = book2, Member = alice, LoanDate = DateTime.Now.AddDays(-2) };
        var loan3 = new Loan { Book = book3, Member = carlos, LoanDate = DateTime.Now.AddDays(-20) };
        var loan4 = new Loan { Book = book4, Member = carlos, LoanDate = DateTime.Now.AddDays(-45) };
        var loan5 = new Loan { Book = book5, Member = alice, LoanDate = DateTime.Now.AddDays(-10) };

        context.Authors.AddRange(orwell, austen);
        context.Loans.AddRange(loan1, loan2, loan3, loan4, loan5);
        context.SaveChanges();
    }

    Console.WriteLine("===== Day 3 Task 7: Relationship Verification =====");

    var authorsWithBooks = context.Authors
        .Include(a => a.Books)
        .ThenInclude(b => b.Genres)
        .ToList();

    foreach (var a in authorsWithBooks)
    {
        Console.WriteLine($"Author: {a.FullName}");
        foreach (var b in a.Books)
        {
            string genreList = string.Join(", ", b.Genres.Select(g => g.Name));
            Console.WriteLine($"  Book: {b.Title} | Genres: {genreList}");
        }
    }

    var membersWithLoans = context.Members
        .Include(m => m.Loans)
        .ThenInclude(l => l.Book)
        .ToList();

    foreach (var m in membersWithLoans)
    {
        Console.WriteLine($"Member: {m.FullName}");
        foreach (var l in m.Loans)
        {
            Console.WriteLine($"  Loan: {l.Book.Title} on {l.LoanDate:yyyy-MM-dd}");
        }
    }
}
// ===== Fin Day 3 Task 7 =====


// ===== Day 3 (Part 2): LINQ Queries - Tasks 1 to 6 =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

    // ---------- TASK 1: Basic Filtering with Where() ----------
    Console.WriteLine();
    Console.WriteLine("===== Task 1: Basic Filtering =====");

    string searchTerm = "Animal";
    var booksByTitle = context.Books
        .AsNoTracking()
        .Where(b => b.Title.Contains(searchTerm))
        .ToList();
    Console.WriteLine($"Query 1A - Books containing '{searchTerm}':");
    foreach (var b in booksByTitle)
        Console.WriteLine($"  {b.Title} ({b.PublicationYear})");

    var recentLoans = context.Loans
        .AsNoTracking()
        .Where(l => l.LoanDate >= DateTime.Now.AddDays(-30))
        .ToList();
    Console.WriteLine($"Query 1B - Loans in the last 30 days: {recentLoans.Count} found");
    foreach (var l in recentLoans)
        Console.WriteLine($"  LoanId {l.LoanId} on {l.LoanDate:yyyy-MM-dd}");

    string emailDomain = "@library.com";
    var membersByDomain = context.Members
        .AsNoTracking()
        .Where(m => m.Email != null && m.Email.EndsWith(emailDomain))
        .ToList();
    Console.WriteLine($"Query 1C - Members with email ending in '{emailDomain}': {membersByDomain.Count} found");
    foreach (var m in membersByDomain)
        Console.WriteLine($"  {m.FullName} - {m.Email}");

    var booksWithGenres = context.Books
        .AsNoTracking()
        .Where(b => b.Genres.Any())
        .ToList();
    Console.WriteLine($"Query 1D - Books with at least one genre: {booksWithGenres.Count} found");
    foreach (var b in booksWithGenres)
        Console.WriteLine($"  {b.Title}");


    // ---------- TASK 2: Projection with Select() ----------
    Console.WriteLine();
    Console.WriteLine("===== Task 2: Projection =====");

    var bookProjections = context.Books
        .AsNoTracking()
        .Select(b => new
        {
            Title = b.Title,
            PublicationYear = b.PublicationYear,
            AuthorName = b.Author.FullName
        })
        .ToList();
    Console.WriteLine("Query 2A - Book title/year/author projection:");
    foreach (var b in bookProjections)
        Console.WriteLine($"  {b.Title} ({b.PublicationYear}) by {b.AuthorName}");

    var loanSummaries = context.Loans
        .AsNoTracking()
        .Select(l => new LoanSummaryDto
        {
            MemberName = l.Member.FullName,
            BookTitle = l.Book.Title,
            LoanDate = l.LoanDate,
            IsOverdue = l.LoanDate < DateTime.Now.AddDays(-14)
        })
        .ToList();
    Console.WriteLine("Query 2B - Loan summaries with overdue flag:");
    foreach (var l in loanSummaries)
        Console.WriteLine($"  {l.MemberName} | {l.BookTitle} | {l.LoanDate:yyyy-MM-dd} | Overdue: {l.IsOverdue}");

    var allGenreNames = context.Books
        .AsNoTracking()
        .SelectMany(b => b.Genres.Select(g => g.Name))
        .Distinct()
        .ToList();
    Console.WriteLine($"Query 2C - Distinct genre names across all books: {string.Join(", ", allGenreNames)}");


    // ---------- TASK 3: Sorting with OrderBy() and ThenBy() ----------
    Console.WriteLine();
    Console.WriteLine("===== Task 3: Sorting =====");

    var booksSorted = context.Books
        .AsNoTracking()
        .OrderByDescending(b => b.PublicationYear)
        .ThenBy(b => b.Title)
        .ToList();
    Console.WriteLine("Query 3A - Books sorted by year desc, title asc:");
    foreach (var b in booksSorted)
        Console.WriteLine($"  {b.Title} ({b.PublicationYear})");

    var loansSorted = context.Loans
        .AsNoTracking()
        .Include(l => l.Member)
        .OrderBy(l => l.Member.FullName)
        .ThenByDescending(l => l.LoanDate)
        .ToList();
    Console.WriteLine("Query 3B - Loans sorted by member name asc, loan date desc:");
    foreach (var l in loansSorted)
        Console.WriteLine($"  {l.Member.FullName} | {l.LoanDate:yyyy-MM-dd}");

    var authorsByBookCount = context.Authors
        .AsNoTracking()
        .Include(a => a.Books)
        .OrderByDescending(a => a.Books.Count)
        .ToList();
    Console.WriteLine("Query 3C - Authors sorted by number of books written (desc):");
    foreach (var a in authorsByBookCount)
        Console.WriteLine($"  {a.FullName} - {a.Books.Count} book(s)");


    // ---------- TASK 4: Combining Filtering, Projection, and Sorting ----------
    Console.WriteLine();
    Console.WriteLine("===== Task 4: Combined Queries =====");

    var activeLoanSummaries = context.Loans
        .AsNoTracking()
        .Where(l => l.LoanDate >= DateTime.Now.AddDays(-60))
        .Select(l => new LoanSummaryDto
        {
            MemberName = l.Member.FullName,
            BookTitle = l.Book.Title,
            LoanDate = l.LoanDate,
            IsOverdue = l.LoanDate < DateTime.Now.AddDays(-14)
        })
        .OrderByDescending(l => l.LoanDate)
        .ToList();
    Console.WriteLine("Query 4A - Active loans (last 60 days), sorted by date desc:");
    foreach (var l in activeLoanSummaries)
        Console.WriteLine($"  {l.MemberName} | {l.BookTitle} | {l.LoanDate:yyyy-MM-dd} | Overdue: {l.IsOverdue}");

    string genreFilter = "Fiction";
    var booksByGenre = context.Books
        .AsNoTracking()
        .Include(b => b.Author)
        .Include(b => b.Genres)
        .Where(b => b.Genres.Any(g => g.Name == genreFilter))
        .Select(b => new
        {
            Title = b.Title,
            AuthorName = b.Author.FullName,
            GenreNames = b.Genres.Select(g => g.Name).ToList()
        })
        .OrderBy(b => b.Title)
        .ToList();
    Console.WriteLine($"Query 4B - Books in genre '{genreFilter}':");
    foreach (var b in booksByGenre)
        Console.WriteLine($"  {b.Title} by {b.AuthorName} | Genres: {string.Join(", ", b.GenreNames)}");


    // ---------- TASK 5: Aggregation Queries ----------
    Console.WriteLine();
    Console.WriteLine("===== Task 5: Aggregation =====");

    var loanCountsByMember = context.Loans
        .AsNoTracking()
        .GroupBy(l => l.Member.FullName)
        .Select(g => new { MemberName = g.Key, LoanCount = g.Count() })
        .OrderByDescending(g => g.LoanCount)
        .ToList();
    Console.WriteLine("Query 5A - Loan count per member (desc):");
    foreach (var g in loanCountsByMember)
        Console.WriteLine($"  {g.MemberName}: {g.LoanCount} loan(s)");

    double averageBooksPerAuthor = context.Authors
        .AsNoTracking()
        .Select(a => a.Books.Count)
        .DefaultIfEmpty()
        .Average();
    Console.WriteLine($"Query 5B - Average number of books per author: {averageBooksPerAuthor:F2}");

    var topBorrower = context.Loans
        .AsNoTracking()
        .GroupBy(l => l.Member.FullName)
        .Select(g => new { MemberName = g.Key, LoanCount = g.Count() })
        .OrderByDescending(g => g.LoanCount)
        .FirstOrDefault();
    if (topBorrower != null)
        Console.WriteLine($"Query 5C - Member with most active loans: {topBorrower.MemberName} ({topBorrower.LoanCount} loans)");
    else
        Console.WriteLine("Query 5C - No loans found.");


    // ---------- TASK 6: Pagination ----------
    Console.WriteLine();
    Console.WriteLine("===== Task 6: Pagination =====");

    List<(int pageNumber, int pageSize)> pageCombinations = new()
    {
        (1, 1),
        (1, 2),
        (2, 1)
    };

    string paginationSearchTerm = "a";

    foreach (var (pageNumber, pageSize) in pageCombinations)
    {
        var baseQuery = context.Books
            .AsNoTracking()
            .Where(b => b.Title.Contains(paginationSearchTerm));

        int totalResults = baseQuery.Count();

        var pagedResults = baseQuery
            .OrderBy(b => b.Title)
            .Select(b => new BookSummaryDto
            {
                Title = b.Title,
                PublicationYear = b.PublicationYear,
                AuthorName = b.Author.FullName
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        Console.WriteLine($"Page {pageNumber} (size {pageSize}) - Total results: {totalResults}");
        foreach (var b in pagedResults)
            Console.WriteLine($"  {b.Title} ({b.PublicationYear}) by {b.AuthorName}");
        Console.WriteLine();
    }
}
// ===== Fin Day 3 (Part 2): LINQ Queries =====


/*
// ===== Day 2 Task 5: Validate the model =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    bool created = context.Database.EnsureCreated();
    Console.WriteLine(created
        ? "Database created successfully."
        : "Database already exists — no changes made.");
}
// ===== Fin Day 2 Task 5 =====
*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();