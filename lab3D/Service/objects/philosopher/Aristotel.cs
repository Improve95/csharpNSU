using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Aristotel(IForkFactory forkFactory, Strategy strategy)
    : Philosopher("Аристотель", forkFactory, strategy);