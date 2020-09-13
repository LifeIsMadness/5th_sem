#include <iostream>
#include <string>
#include <mutex>
#include <thread>
#include <atomic>
#include <chrono>
#include <queue>

__interface IQueue
{
public:
	virtual void push(uint8_t val) = 0;
	virtual bool pop(uint8_t &val) = 0;
};

class ThreadSafeQueue: public IQueue
{
private:
	std::queue<uint16_t> rawQueue;
	std::mutex queueMutex;

public:

	void push(uint8_t val)
	{
		queueMutex.lock();
		rawQueue.push(val);
		queueMutex.unlock();
	}

	bool pop(uint8_t &val)
	{

	}
};
