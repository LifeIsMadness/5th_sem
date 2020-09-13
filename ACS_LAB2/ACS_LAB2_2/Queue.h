#include <iostream>
#include <string>
#include <mutex>
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
	std::queue<uint8_t> rawQueue;
	std::mutex queueMutex;

public:
	ThreadSafeQueue()
	{
		rawQueue.push(1);
		rawQueue.push(2);
		rawQueue.push(3);
	}


	void push(uint8_t val)
	{
		queueMutex.lock();
		rawQueue.push(val);
		queueMutex.unlock();
	}

	bool pop(uint8_t &val)
	{
		if (rawQueue.empty())
		{
			std::this_thread::sleep_for(std::chrono::milliseconds(1));
		}

		std::lock_guard<std::mutex> lock(queueMutex);
		if (!rawQueue.empty())
		{
			val = rawQueue.front();
			rawQueue.pop();
			//queueMutex.unlock();
			return true;
		}
		//queueMutex.unlock();
		return false;
	}
};
