﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test2.Models;

public partial class Test2Context : DbContext
{
    public Test2Context()
    {
    }

    public Test2Context(DbContextOptions<Test2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityType> ActivityTypes { get; set; }

    // 新增 ActivityRegistrations
    public virtual DbSet<ActivityRegistration> ActivityRegistrations { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<AnnouncementType> AnnouncementTypes { get; set; }

    public virtual DbSet<Audience> Audiences { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookStatus> BookStatuses { get; set; }

    public virtual DbSet<Borrow> Borrows { get; set; }

    public virtual DbSet<BorrowStatus> BorrowStatuses { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservationStatus> ReservationStatuses { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    //custom
    public virtual DbSet<Type> cId { get; set; }
    public virtual DbSet<Type> collectionId { get; set; }
    public virtual DbSet<Type> reservationDate { get; set; }
    public virtual DbSet<Type> reservationStatusId { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=test2;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("Activity");

            entity.HasIndex(e => e.ActivityTitle, "UQ__Activity__5F9FE5733FADB099").IsUnique();

            entity.Property(e => e.ActivityId).HasColumnName("activityId");
            entity.Property(e => e.ActivityDesc).HasColumnName("activityDesc");
            entity.Property(e => e.ActivityImg).HasColumnName("activityImg");
            entity.Property(e => e.ActivityTitle)
                .HasMaxLength(100)
                .HasColumnName("activityTitle");
            entity.Property(e => e.ActivityTypeId).HasColumnName("activityTypeId");
            entity.Property(e => e.AudienceId).HasColumnName("audienceId");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.StartDate).HasColumnName("startDate");

            entity.HasOne(d => d.ActivityType).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ActivityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityType");

            entity.HasOne(d => d.Audience).WithMany(p => p.Activities)
                .HasForeignKey(d => d.AudienceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audience");
        });

        modelBuilder.Entity<ActivityType>(entity =>
        {
            entity.ToTable("ActivityType");

            entity.HasIndex(e => e.ActivityType1, "UQ__Activity__1F1EE4DDCFFFAE6E").IsUnique();

            entity.Property(e => e.ActivityTypeId).HasColumnName("activityTypeId");
            entity.Property(e => e.ActivityType1)
                .HasMaxLength(50)
                .HasColumnName("activityType");
        });

        // 新增 ActivityRegistrations

        modelBuilder.Entity<ActivityRegistration>(entity =>
        {
            entity.ToTable("ActivityRegistrations"); // 指定對應的表格名稱

            // 設定主鍵
            entity.HasKey(e => e.ActivityRegistrationId);

            // 設定外來鍵 ClientId 與 Client 表格的關聯
            entity.HasOne(ar => ar.Client)
                  .WithMany(c => c.ActivityRegistrations) // Client Model 中的導覽屬性名稱
                  .HasForeignKey(ar => ar.ClientId)
                  .OnDelete(DeleteBehavior.Cascade) // 與 SQL 的 ON DELETE CASCADE 一致
                  .HasConstraintName("FK_ActivityRegistrations_Client"); // 與 SQL 的 CONSTRAINT 名稱一致

            // 設定外來鍵 ActivityId 與 Activity 表格的關聯
            entity.HasOne(ar => ar.Activity)
                  .WithMany(a => a.ActivityRegistrations) // Activity Model 中的導覽屬性名稱
                  .HasForeignKey(ar => ar.ActivityId)
                  .OnDelete(DeleteBehavior.Cascade) // 與 SQL 的 ON DELETE CASCADE 一致
                  .HasConstraintName("FK_ActivityRegistrations_Activity"); // 與 SQL 的 CONSTRAINT 名稱一致

            // 設定複合唯一索引 (UQ_ActivityRegistrations_ClientActivity)
            entity.HasIndex(ar => new { ar.ClientId, ar.ActivityId })
                  .IsUnique()
                  .HasDatabaseName("UQ_ActivityRegistrations_ClientActivity"); // 與 SQL 的 CONSTRAINT 名稱一致

            // 設定欄位映射和屬性 (如果 Model 中的 Data Annotations 不夠，或需要更詳細的設定)
            entity.Property(e => e.ActivityRegistrationId).HasColumnName("ActivityRegistrationId");
            entity.Property(e => e.ClientId).HasColumnName("ClientId");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityId");
            entity.Property(e => e.ActivityRegistrationDate)
                  .HasColumnName("ActivityRegistrationDate")
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("GETDATE()"); // 與 SQL 的 DEFAULT GETDATE() 一致

            entity.Property(e => e.Status)
                  .HasColumnName("Status")
                  .HasMaxLength(50) // 與 SQL 的 NVARCHAR(50) 一致
                  .HasDefaultValue("已報名"); // 與 SQL 的 DEFAULT '已報名' 一致 (EF Core 會轉換成 SQL)
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("Announcement");

            entity.HasIndex(e => e.AnnouncementTitle, "UQ__Announce__EBE0015F579A2769").IsUnique();

            entity.Property(e => e.AnnouncementId).HasColumnName("announcementId");
            entity.Property(e => e.AnnouncementDesc).HasColumnName("announcementDesc");
            entity.Property(e => e.AnnouncementTitle)
                .HasMaxLength(100)
                .HasColumnName("announcementTitle");
            entity.Property(e => e.AnnouncementTypeId).HasColumnName("announcementTypeId");

            entity.HasOne(d => d.AnnouncementType).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.AnnouncementTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnnouncementType");
        });

        modelBuilder.Entity<AnnouncementType>(entity =>
        {
            entity.ToTable("AnnouncementType");

            entity.HasIndex(e => e.AnnouncementType1, "UQ__Announce__D53C87B22984DEC9").IsUnique();

            entity.Property(e => e.AnnouncementTypeId).HasColumnName("announcementTypeId");
            entity.Property(e => e.AnnouncementType1)
                .HasMaxLength(50)
                .HasColumnName("announcementType");
        });

        modelBuilder.Entity<Audience>(entity =>
        {
            entity.ToTable("Audience");

            entity.HasIndex(e => e.Audience1, "UQ__Audience__2C1B51FCF15D941B").IsUnique();

            entity.Property(e => e.AudienceId).HasColumnName("audienceId");
            entity.Property(e => e.Audience1)
                .HasMaxLength(50)
                .HasColumnName("audience");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");

            entity.HasIndex(e => e.Author1, "UQ_Author_Name").IsUnique();

            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.Author1)
                .HasMaxLength(50)
                .HasColumnName("author");
            entity.Property(e => e.AuthorDesc).HasColumnName("authorDesc");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.HasIndex(e => e.BookCode, "UQ__Book__3BB8DAE6F11F54D2").IsUnique();

            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.AccessionDate).HasColumnName("accessionDate");
            entity.Property(e => e.BookCode)
                .HasMaxLength(23)
                .IsUnicode(false)
                .HasColumnName("bookCode");
            entity.Property(e => e.BookStatusId).HasColumnName("bookStatusId");
            entity.Property(e => e.CollectionId).HasColumnName("collectionId");

            entity.HasOne(d => d.BookStatus).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookStatus");

            entity.HasOne(d => d.Collection).WithMany(p => p.Books)
                .HasForeignKey(d => d.CollectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CollectionB");
        });

        modelBuilder.Entity<BookStatus>(entity =>
        {
            entity.ToTable("BookStatus");

            entity.HasIndex(e => e.BookStatus1, "UQ__BookStat__6890475C2DC102B3").IsUnique();

            entity.Property(e => e.BookStatusId).HasColumnName("bookStatusId");
            entity.Property(e => e.BookStatus1)
                .HasMaxLength(50)
                .HasColumnName("bookStatus");
        });

        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.ToTable("Borrow");

            entity.Property(e => e.BorrowId).HasColumnName("borrowId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.BorrowDate).HasColumnName("borrowDate");
            entity.Property(e => e.BorrowStatusId).HasColumnName("borrowStatusId");
            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.DueDateB).HasColumnName("dueDateB");
            entity.Property(e => e.ReservationId).HasColumnName("reservationId");
            entity.Property(e => e.ReturnDate).HasColumnName("returnDate");

            entity.HasOne(d => d.Book).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookB");

            entity.HasOne(d => d.BorrowStatus).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.BorrowStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BorrowStatus");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.CId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientB");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK_Reservation");
        });

        modelBuilder.Entity<BorrowStatus>(entity =>
        {
            entity.ToTable("BorrowStatus");

            entity.HasIndex(e => e.BorrowStatus1, "UQ__BorrowSt__EF08F8F4B409EFC7").IsUnique();

            entity.Property(e => e.BorrowStatusId).HasColumnName("borrowStatusId");
            entity.Property(e => e.BorrowStatus1)
                .HasMaxLength(50)
                .HasColumnName("borrowStatus");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("Client");

            entity.HasIndex(e => e.CPhone, "UQ__Client__0A376ADC5C14DAE8").IsUnique();

            entity.HasIndex(e => e.CAccount, "UQ__Client__2F046D2399440AC9").IsUnique();

            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.CAccount)
                .HasMaxLength(100)
                .HasColumnName("cAccount");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasColumnName("cName");
            entity.Property(e => e.CPassword)
                .HasMaxLength(255)
                .HasColumnName("cPassword");
            entity.Property(e => e.CPhone)
                .HasMaxLength(20)
                .HasColumnName("cPhone");
            entity.Property(e => e.Permission).HasColumnName("permission");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("Collection");

            entity.HasIndex(e => e.Isbn, "UQ__Collecti__99F9D0A4B36171AF").IsUnique();

            entity.Property(e => e.CollectionId).HasColumnName("collectionId");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.CollectionDesc).HasColumnName("collectionDesc");
            entity.Property(e => e.CollectionImg).HasColumnName("collectionImg");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.PublishDate).HasColumnName("publishDate");
            entity.Property(e => e.Publisher)
                .HasMaxLength(50)
                .HasColumnName("publisher");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Translator)
                .HasMaxLength(50)
                .HasColumnName("translator");
            entity.Property(e => e.TypeId).HasColumnName("typeId");

            entity.HasOne(d => d.Author).WithMany(p => p.Collections)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Author");

            entity.HasOne(d => d.Language).WithMany(p => p.Collections)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Language");

            entity.HasOne(d => d.Type).WithMany(p => p.Collections)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Type");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoritesId);

            entity.HasIndex(e => new { e.CId, e.CollectionId }, "CUC_Favorites").IsUnique();

            entity.Property(e => e.FavoritesId).HasColumnName("favoritesId");
            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.CollectionId).HasColumnName("collectionId");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.CId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientF");

            entity.HasOne(d => d.Collection).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.CollectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CollectionF");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.ToTable("History");

            entity.Property(e => e.HistoryId).HasColumnName("historyId");
            entity.Property(e => e.BorrowId).HasColumnName("borrowId");
            entity.Property(e => e.Feedback).HasColumnName("feedback");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Borrow).WithMany(p => p.Histories)
                .HasForeignKey(d => d.BorrowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Borrow");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("Language");

            entity.HasIndex(e => e.Language1, "UQ__Language__EFADA5D93D30595A").IsUnique();

            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Language1)
                .HasMaxLength(50)
                .HasColumnName("language");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notificationId");
            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.NotificationDate).HasColumnName("notificationDate");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientN");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("Reservation");

            entity.Property(e => e.ReservationId).HasColumnName("reservationId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.CollectionId).HasColumnName("collectionId");
            entity.Property(e => e.DueDateR).HasColumnName("dueDateR");
            entity.Property(e => e.ReservationDate).HasColumnName("reservationDate");
            entity.Property(e => e.ReservationStatusId).HasColumnName("reservationStatusId");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_BookR");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.CId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientR");

            entity.HasOne(d => d.Collection).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.CollectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CollectionR");

            entity.HasOne(d => d.ReservationStatus).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.ReservationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationStatus");
        });

        modelBuilder.Entity<ReservationStatus>(entity =>
        {
            entity.ToTable("ReservationStatus");

            entity.HasIndex(e => e.ReservationStatus1, "UQ__Reservat__C351879309FCEE92").IsUnique();

            entity.Property(e => e.ReservationStatusId).HasColumnName("reservationStatusId");
            entity.Property(e => e.ReservationStatus1)
                .HasMaxLength(50)
                .HasColumnName("reservationStatus");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.ToTable("Type");

            entity.HasIndex(e => e.Type1, "UQ__Type__E3F8524832E04BDF").IsUnique();

            entity.Property(e => e.TypeId).HasColumnName("typeId");
            entity.Property(e => e.Type1)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
