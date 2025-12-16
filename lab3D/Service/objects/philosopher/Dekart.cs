using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Dekart(IForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Декарт",  forkFactory, strategy) {}