using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PsiogPaisaAPI.Models;

public partial class PsiogpaisaContext : DbContext
{
    public PsiogpaisaContext()
    {
    }

    public PsiogpaisaContext(DbContextOptions<PsiogpaisaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<External> Externals { get; set; }

    public virtual DbSet<ExternalType> ExternalTypes { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<IndividualType> IndividualTypes { get; set; }

    public virtual DbSet<LendBack> LendBacks { get; set; }

    public virtual DbSet<MasterAccount> MasterAccounts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<PayType> PayTypes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<SelfWallet> SelfWallet { get; set; }

    public virtual DbSet<Statement> Statements { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PSILENL137;Database=psiogpaisa;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("Employee_pk");

            entity.ToTable("Employee");

            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmpFname)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("emp_fname");
            entity.Property(e => e.EmpLname)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("emp_lname");
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<External>(entity =>
        {
            entity.HasKey(e => e.ExtId).HasName("External_pk");

            entity.ToTable("External", tb => tb.HasTrigger("TriggerExternalInsert"));

            entity.Property(e => e.ExtId).HasColumnName("ext_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Emp).WithMany(p => p.Externals)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("External_Employee");

            entity.HasOne(d => d.Type).WithMany(p => p.Externals)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("External_External_type");
        });

        modelBuilder.Entity<ExternalType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("External_type_pk");

            entity.ToTable("External_type");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("type_id");
            entity.Property(e => e.Typename)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("typename");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("Grp_Pk");

            entity.ToTable("Group", tb => tb.HasTrigger("TriggerGroupInsert"));

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ContributorId).HasColumnName("contributor_id");
            entity.Property(e => e.ReqId).HasColumnName("req_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Contributor).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ContributorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Group_Employee");

            entity.HasOne(d => d.Req).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ReqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Group_Request");

            entity.HasOne(d => d.Status).WithMany(p => p.Groups)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Group_Status");

            entity.HasOne(d => d.Type).WithMany(p => p.Groups)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Group_Pay_Type");
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.HasKey(e => e.IndId).HasName("Individual_pk");

            entity.ToTable("Individual", tb => tb.HasTrigger("TriggerIndividualInsert"));

            entity.Property(e => e.IndId).HasColumnName("ind_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.RecieverId).HasColumnName("reciever_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Reciever).WithMany(p => p.IndividualRecievers)
                .HasForeignKey(d => d.RecieverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OneToOne_Transaction_Employee");

            entity.HasOne(d => d.Sender).WithMany(p => p.IndividualSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OneToOne_Transaction1_Employee");

            entity.HasOne(d => d.Status).WithMany(p => p.Individuals)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Individual_Status");

            entity.HasOne(d => d.Type).WithMany(p => p.Individuals)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OneToOne_Transaction_OneToOne_TransactionType");
        });

        modelBuilder.Entity<IndividualType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("Individual_Type_pk");

            entity.ToTable("Individual_Type");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("type_id");
            entity.Property(e => e.Typename)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("typename");
        });

        modelBuilder.Entity<LendBack>(entity =>
        {
            entity.HasKey(e => e.LendId).HasName("LendBack_pk");

            entity.ToTable("LendBack");

            entity.Property(e => e.LendId).HasColumnName("lend_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.PaybackAmount).HasColumnName("payback_amount");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Group).WithMany(p => p.LendBacks)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lendbk_grp");

            entity.HasOne(d => d.Type).WithMany(p => p.LendBacks)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LendBack_Pay_Type");
        });

        modelBuilder.Entity<MasterAccount>(entity =>
        {
            entity.HasKey(e => e.MasterId).HasName("MasterAccount_pk");

            entity.ToTable("MasterAccount");

            entity.Property(e => e.MasterId)
                .ValueGeneratedNever()
                .HasColumnName("master_id");
            entity.Property(e => e.EmployeeEmpId).HasColumnName("Employee_emp_id");

            entity.HasOne(d => d.EmployeeEmp).WithMany(p => p.MasterAccounts)
                .HasForeignKey(d => d.EmployeeEmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MasterAccount_Employee");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("Notification_pk");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("Notification_ID");
            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.NotificationTypeId).HasColumnName("Notification_typeID");

            entity.HasOne(d => d.Emp).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notification_Employee");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notification_NotificationType");
        });

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.HasKey(e => e.NotificationTypeId).HasName("NotificationType_pk");

            entity.ToTable("NotificationType");

            entity.Property(e => e.NotificationTypeId)
                .ValueGeneratedNever()
                .HasColumnName("Notification_typeID");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<PayType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("Pay_Type_pk");

            entity.ToTable("Pay_Type");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("type_id");
            entity.Property(e => e.Typename)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("typename");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("Request_pk");

            entity.ToTable("Request");

            entity.Property(e => e.ReqId).HasColumnName("req_id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.Purpose)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("purpose");
            entity.Property(e => e.QuotedAmount).HasColumnName("quoted_amount");
            entity.Property(e => e.RecievedAmount).HasColumnName("recieved_amount");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Emp).WithMany(p => p.Requests)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Request_Employee");

            entity.HasOne(d => d.Status).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Request_Status");
        });

        modelBuilder.Entity<SelfWallet>(entity =>
        {
            entity.HasKey(e => e.WalId).HasName("Self_Wallet_pk");

            entity.ToTable("Self_Wallet", tb => tb.HasTrigger("TriggerSelfWalletUpdate"));

            entity.Property(e => e.WalId)
                .ValueGeneratedNever()
                .HasColumnName("wal_id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.WalletAmount).HasColumnName("wallet_amount");

            entity.HasOne(d => d.Emp).WithMany(p => p.SelfWallets)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Wallet_Employee");

            entity.HasOne(d => d.Status).WithMany(p => p.SelfWallets)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Self_Wallet_Status");
        });

        modelBuilder.Entity<Statement>(entity =>
        {
            entity.HasKey(e => e.TransId).HasName("Statement_pk");

            entity.ToTable("Statement");

            entity.Property(e => e.TransId)
                .ValueGeneratedNever()
                .HasColumnName("trans_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CdOrDb)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("cd_OR_db");
            entity.Property(e => e.ExtId).HasColumnName("ext_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.IndId).HasColumnName("ind_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Typename)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("typename");

            entity.HasOne(d => d.Ext).WithMany(p => p.Statements)
                .HasForeignKey(d => d.ExtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MasterTransaction_External");

            entity.HasOne(d => d.Group).WithMany(p => p.Statements)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MasTran_grp");

            entity.HasOne(d => d.Ind).WithMany(p => p.Statements)
                .HasForeignKey(d => d.IndId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MasterTransaction_Individual");

            entity.HasOne(d => d.Status).WithMany(p => p.Statements)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MasterTransaction_Status");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("Status_pk");

            entity.ToTable("Status");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("status_id");
            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
