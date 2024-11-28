﻿using EntityPoste;
using EntityPoste.Repository;
using EntityPoste.SeedWork;

var appDbContext = new AppDbContext();
IUserRepository userRepository = new UserRepository(appDbContext);
using var unitOfWork = new Printer(userRepository);

unitOfWork.Work();