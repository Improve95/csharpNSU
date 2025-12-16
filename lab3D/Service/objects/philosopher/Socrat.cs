using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Socrat(IForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Сократ", forkFactory, strategy) {}