﻿using Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<TodoDbModel> Todos { get; set; }

    }
}
