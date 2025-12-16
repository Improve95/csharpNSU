using IService.service;
using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Socrat(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Сократ", tableManager, strategy) {}