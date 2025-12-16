using IService.service;
using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Aristotel(ITableManager tableManager, Strategy strategy)
    : Philosopher("Аристотель", tableManager, strategy);